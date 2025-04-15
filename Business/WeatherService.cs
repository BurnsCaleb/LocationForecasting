// Contains all classes needed for retrieving weather forecast data

using Newtonsoft.Json;

namespace LocationForecasting.Models
{
    public class WeatherService
    {
        // Base url for api request
        private const string ApiUrl = "https://api.weather.gov/points/";

        // API Header Information
        private const string ProjectName = "LocationForecasting.com";
        private const string Contact = "burnscaleb1415@gmail.com";

        /// <summary>
        /// Communicate with National Weather Service api to a get a location's forecast
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<WeatherRequest[]> GetWeather(double latitude, double longitude)
        {
            //Api Url with latitude longitude
            string gridUrl = $"{ApiUrl}{latitude},{longitude}";


            using (HttpClient client = new HttpClient())
            {
                // Set Header
                client.DefaultRequestHeaders.Add("User-Agent", $"{ProjectName} (Contact: {Contact})");

                // Get Grid Position
                HttpResponseMessage gridResponse = await client.GetAsync(gridUrl);
                if (!gridResponse.IsSuccessStatusCode)
                {
                    // Handle Error
                    throw new HttpRequestException();
                }

                // Gather returned data and create GridResponse object
                string gridReturned = await gridResponse.Content.ReadAsStringAsync();
                GridResponse gridData = JsonConvert.DeserializeObject<GridResponse>(gridReturned);
                string forecastUrl = gridData.Properties.Forecast;

                // Get Forecast For Grid
                HttpResponseMessage forecastResponse = await client.GetAsync(forecastUrl);
                if (!forecastResponse.IsSuccessStatusCode)
                {
                    // Handle Error
                    throw new Exception("Unable to retrieve forecast information\n" +
                        $" Forecast Url: {gridData.Properties.Forecast}");
                }

                // Gather returned data and create forecast object
                string forecastReturned = await forecastResponse.Content.ReadAsStringAsync();

                ForecastResponse forecastData = JsonConvert.DeserializeObject<ForecastResponse>(forecastReturned);

                // Create a list of Weather Request Objects
                // Size = 14
                WeatherRequest[] weatherData = new WeatherRequest[14];
                for (int i = 0; i < forecastData.Properties.Periods.Length; i++)
                {
                    weatherData[i] = new WeatherRequest
                    {
                        Forecast = forecastData.Properties.Periods[i].Name,
                        IsDayTime = forecastData.Properties.Periods[i].IsDayTime,
                        Temperature = forecastData.Properties.Periods[i].Temperature,
                        TemperatureUnit = forecastData.Properties.Periods[i].TemperatureUnit,
                        Icon = forecastData.Properties.Periods[i].Icon,
                        DetailedForecast = forecastData.Properties.Periods[i].DetailedForecast,
                        RequestTime = DateTime.Now,
                    };
                }

                return weatherData;
            }
        }
        
        public class WeatherRequest
        {
            public string Forecast { get; set; }
            public bool IsDayTime { get; set; }
            public double Temperature { get; set; }
            public string TemperatureUnit { get; set; }
            public string Icon { get; set; }
            public string DetailedForecast { get; set; }
            public DateTime RequestTime { get; set; }
        }

        public class GridResponse
        {
            public GridProperties Properties { get; set; }
        }

        public class GridProperties
        {
            public string Forecast { get; set; }
        }

        public class ForecastResponse
        {
            public ForecastProperties Properties { get; set; }
        }

        public class ForecastProperties
        {
            public TimePeriod[] Periods { get; set; }
        }

        public class TimePeriod
        {
            public string Name { get; set; }
            public bool IsDayTime { get; set; }
            public double Temperature { get; set; }
            public string TemperatureUnit { get; set; }
            public string Icon { get; set; }
            public string DetailedForecast { get; set; }
        }
    }
}
