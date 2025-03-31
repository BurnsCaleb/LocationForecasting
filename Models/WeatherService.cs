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
                        "Grid:\n" +
                        $" Grid Id: {gridData.Properties.GridId}\n" +
                        $" Grid X: {gridData.Properties.GridX}\n" +
                        $" Grid Y: {gridData.Properties.GridY}\n" +
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
                        WindSpeed = forecastData.Properties.Periods[i].WindSpeed,
                        WindDirection = forecastData.Properties.Periods[i].WindDirection,
                        ShortForecast = forecastData.Properties.Periods[i].ShortForecast,
                        DetailedForecast = forecastData.Properties.Periods[i].DetailedForecast,
                        ForecastUrl = forecastUrl,
                        RequestTime = DateTime.Now,
                    };
                }

                return weatherData;
            }
        }
        public class WeatherRequest
        {
            // Properties
            private string _forecast;
            private bool _isDayTime;
            private double _temperature;
            private string _temperatureUnit;
            private string _windSpeed;
            private string _windDirection;
            private string _shortForecast;
            private string _detailedForecast;
            private string _forecastUrl;
            private DateTime _requestTime;

            // Access
            public string Forecast { get { return _forecast; } set { _forecast = value; } }
            public bool IsDayTime { get { return _isDayTime; } set { _isDayTime = value; } }
            public double Temperature { get { return _temperature; } set { _temperature = value; } }
            public string TemperatureUnit { get { return _temperatureUnit; } set { _temperatureUnit = value; } }
            public string WindSpeed { get { return _windSpeed; } set { _windSpeed = value; } }
            public string WindDirection { get { return _windDirection; } set { _windDirection = value; } }
            public string ShortForecast { get { return _shortForecast; } set { _shortForecast = value; } }
            public string DetailedForecast { get { return _detailedForecast; } set { _detailedForecast = value; } }
            public string ForecastUrl { get { return _forecastUrl; } set { _forecastUrl = value; } }
            public DateTime RequestTime { get { return _requestTime; } set { _requestTime = value; } }
        }

        public class GridResponse
        {
            private GridProperties _properties;

            public GridProperties Properties { get { return _properties; } set { _properties = value; } }
        }

        public class GridProperties
        {
            private string _gridId;
            private int _gridX;
            private int _gridY;
            private string _forecast;

            public string GridId { get { return _gridId; } set { _gridId = value; } }
            public int GridX { get { return _gridX; } set { _gridX = value; } }
            public int GridY { get { return _gridY; } set { _gridY = value; } }
            public string Forecast { get { return _forecast; } set { _forecast = value; } }
        }

        public class ForecastResponse
        {
            private ForecastProperties _properties;
            public ForecastProperties Properties { get { return _properties; } set { _properties = value; } }
        }

        public class ForecastProperties
        {
            private TimePeriod[] _periods;

            public TimePeriod[] Periods { get { return _periods; } set { _periods = value; } }
        }

        public class TimePeriod
        {
            private string _name;
            private bool _isDayTime;
            private double _temperature;
            private string _temperatureUnit;
            private string _windSpeed;
            private string _windDirection;
            private string _shortForecast;
            private string _detailedForecast;

            public string Name { get { return _name; } set { _name = value; } }
            public bool IsDayTime { get { return _isDayTime; } set { _isDayTime = value; } }
            public double Temperature { get { return _temperature; } set { _temperature = value; } }
            public string TemperatureUnit { get { return _temperatureUnit; } set { _temperatureUnit = value; } }
            public string WindSpeed { get { return _windSpeed; } set { _windSpeed = value; } }
            public string WindDirection { get { return _windDirection; } set { _windDirection = value; } }
            public string ShortForecast { get { return _shortForecast; } set { _shortForecast = value; } }
            public string DetailedForecast { get { return _detailedForecast; } set { _detailedForecast = value; } }
        }
    }
}
