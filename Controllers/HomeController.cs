using System.Diagnostics;
using LocationForecasting.Data;
using LocationForecasting.Models;
using Microsoft.AspNetCore.Mvc;

namespace LocationForecasting.Controllers
{
    public class HomeController : Controller
    {
        // Database Operations
        private readonly RequestData _requestData;
        private readonly ExceptionData _exceptionData;
        private readonly SettingsData _settingsData;

        // Create new WeatherService and LocationService object
        private WeatherService _weatherService;
        private LocationService _locationService;

        public HomeController(RequestData requestData, ExceptionData exceptionData, SettingsData settingsData, WeatherService weatherService, LocationService locationService)
        {
            _requestData = requestData;
            _exceptionData = exceptionData;
            _settingsData = settingsData;
            _weatherService = weatherService;
            _locationService = locationService;
        }

        public IActionResult Index()
        {
            return View();
        }


        // API Requests

        /// <summary>
        /// Grabs forecast for a location from National Weather Service API
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

                _requestData.LogRequest(weather);

                return Json(weather);
            }

            // Handle Exceptions
            catch (HttpRequestException re)
            {
                _exceptionData.LogException(re);

                Debug.WriteLine(re.Message);
                return Json(new { error = "Failed retrieving weather data. Please ensure selected location is within the United States." });
            }
            catch (Exception e)
            {
                _exceptionData.LogException(e);

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
                // Get ApiKey from Database
                string apiKey = _settingsData.GetApiKey();
                Console.WriteLine(apiKey);

                // Call LocationService.GetLocation to get City and State
                LocationService.LocationData location = await _locationService.GetLocation(coords.Latitude, coords.Longitude, apiKey);

                return Json(location);
            }
            catch (Exception e)
            {
                _exceptionData.LogException(e); 

                Debug.WriteLine(e.Message);
                return Json(new { error = "Unexpected Error. Please try again." });
            }
        }

        public class Coordinates
        {
            private double _latitude;
            private double _longitude;

            public double Latitude { get { return _latitude; } set { _latitude = value; } }
            public double Longitude { get { return _longitude; } set { _longitude = value; } }
        }
    }
}
