using System.Diagnostics;
using LocationForecasting.Data;
using LocationForecasting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LocationForecasting.Controllers
{
    public class HomeController : Controller
    {
        private readonly LocationForecastingContext _context;

        // Create new WeatherService and LocationService object
        private WeatherService _weatherService;
        private LocationService _locationService;

        public HomeController(LocationForecastingContext context)
        {
            _weatherService = new WeatherService();
            _locationService = new LocationService();

            // Database Context
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Creates or retrieves UserId cookie and creates a new user in the database if needed
        /// </summary>
        /// <returns>string UserId</returns>
        private async Task<string> getUserIdCookie()
        {
            // Grab userId Cookie
            string userId = Request.Cookies["UserId"];

            // Check if Cookie exists
            if (string.IsNullOrWhiteSpace(userId))
            {
                // New UserId Cookie
                userId = Guid.NewGuid().ToString();
                Response.Cookies.Append("UserId", userId, new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddYears(1)
                });
            }

            // Create user in database if needed
            bool userExists = await _context.Users.AnyAsync(u => u.UserId == userId);
            if (!userExists)
            {
                User newUser = new User { UserId = userId };
                await SaveUser(newUser);
            }

            return userId;
        }

        // API Requests

        /// <summary>
        /// Grabs forecast for a location from National Weather Service
        /// </summary>
        /// <param name="coords"></param>
        /// <returns>WeatherRequest[] weather</returns>
        [HttpPost]
        public async Task<IActionResult> GetWeather([FromBody] Coordinates coords)
        {
            if (coords == null || coords.Latitude == 0 || coords.Longitude == 0)
            {
                return BadRequest("Invalid coordinates");
            }

            try
            {
                // Calls WeatherService.GetWeather and receives a list of forecasts
                WeatherService.WeatherRequest[] weather = await _weatherService.GetWeather(coords.Latitude, coords.Longitude);

                // Save Data
                await SaveRequest(new RequestLog { 
                    UserId = (await getUserIdCookie()),
                    Latitude = coords.Latitude,
                    Longitude = coords.Longitude,
                    RequestUrl = weather[0].ForecastUrl,
                    ResponseStatus = "OK",
                    ResponseData = JsonConvert.SerializeObject(weather[0])
                });

                return Json(weather);
            }

            // Handle Exceptions
            catch (HttpRequestException re)
            {
                await SaveException(new ExceptionLog
                {
                    UserId = (await getUserIdCookie()),
                    ExceptionMessage = re.Message,
                    StackTrace = re.StackTrace,
                });
                Debug.WriteLine(re.Message);
                return Json(new { error = "Failed retrieving weather data. Please ensure selected location is within the United States." });
            }
            catch (Exception e)
            {
                await SaveException(new ExceptionLog
                {
                    UserId = (await getUserIdCookie()),
                    ExceptionMessage = e.Message,
                    StackTrace = e.StackTrace,
                });
                Debug.WriteLine(e.Message);
                return Json(new { error = "An unexpected error occurred. Please try again later." });
            }
        }

        /// <summary>
        /// Grabs location name from Google Maps
        /// </summary>
        /// <param name="coords"></param>
        /// <returns>LocationData location</returns>
        [HttpPost]
        public async Task<IActionResult> GetLocation([FromBody] Coordinates coords)
        {
            if (coords == null || coords.Latitude == 0 || coords.Longitude == 0)
            {
                return BadRequest("Invalid coordinates");
            }

            try
            {
                // Call LocationService.GetLocation to get City and State
                LocationService.LocationData location = await _locationService.GetLocation(coords.Latitude, coords.Longitude, GetApiKey().Result);

                return Json(location);
            }

            // Handle Exceptions
            catch (HttpRequestException re)
            {
                await SaveException(new ExceptionLog
                {
                    UserId = (await getUserIdCookie()),
                    ExceptionMessage = re.Message,
                    StackTrace = re.StackTrace,
                });
                Debug.WriteLine(re.Message);
                return Json(new { error = "Unexpected Error. Please try again." });
            }
            catch (Exception e)
            {
                await SaveException(new ExceptionLog
                {
                    UserId = (await getUserIdCookie()),
                    ExceptionMessage = e.Message,
                    StackTrace = e.StackTrace,
                });
                Debug.WriteLine(e.Message);
                return Json(new { error = "Unexpected Error. Please try again." });
            }
        }

        public async Task<string> GetApiKey()
        {
            var apiKey = await _context.KeyStorage.Select(k => k.Key).FirstOrDefaultAsync();
            return apiKey;
        }

        public class Coordinates
        {
            private double _latitude;
            private double _longitude;

            public double Latitude { get { return _latitude; } set { _latitude = value; } }
            public double Longitude { get { return _longitude; } set { _longitude = value; } }
        }

        // Database Functions

        /// <summary>
        /// Adds a new User to the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveUser([FromBody] User user)
        {
            try
            {
                // Add User
                _context.Users.Add(user);
                return Json(0);
            }

            // Handle Exception
            catch (Exception ex)
            {
                await SaveException(new ExceptionLog
                {
                    UserId = (await getUserIdCookie()),
                    ExceptionMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                });
                Debug.WriteLine(ex.Message);
                return StatusCode(500, "Error saving user data.");
            }
        }
        
        /// <summary>
        /// Adds a new Request Log to the database
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveRequest([FromBody] RequestLog log)
        {
            if (log == null || log.RequestUrl == null || log.ResponseData == null || log.ResponseStatus == null)
            {
                return BadRequest("Invalid RequestLog Data");
            }

            try
            {
                // Add Request Log
                _context.RequestLogs.Add(log);
                await _context.SaveChangesAsync();
                Debug.WriteLine("Request Log Saved Successfully");
                return Json(0);
            }

            // Handle Exceptions
            catch (Exception ex)
            {
                await SaveException(new ExceptionLog
                {
                    UserId = (await getUserIdCookie()),
                    ExceptionMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                });
                Debug.WriteLine(ex.Message);
                return StatusCode(500, "Error saving location data.");
            }
        }
        
        /// <summary>
        /// Adds a new Exception Log to the Database
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveException([FromBody] ExceptionLog log)
        {
            if (log == null || log.ExceptionMessage == null)
            {
                return BadRequest("Invalid ExceptionLog Data");
            }

            if (log.StackTrace == null)
            {
                log.StackTrace = "No stack trace";
            }

            try
            {
                // Check if user exists in database
                // If not create new user
                bool userExists = await _context.Users.AnyAsync(u => u.UserId == log.UserId);
                if (!userExists)
                {
                    _context.Users.Add(new Models.User { UserId = (await getUserIdCookie()) });
                }

                // Save Exception Log
                _context.ExceptionLogs.Add(log);
                await _context.SaveChangesAsync();
                Debug.WriteLine("Exception Log Saved Successfully");
                return Json(0);
            }

            // Handle Exception
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return StatusCode(500, "Error saving settings data.");
            }
        }
    }
}
