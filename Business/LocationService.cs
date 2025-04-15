// Contains all classes needed for retrieving location data

using Newtonsoft.Json;

namespace LocationForecasting.Models
{
    public class LocationService
    {
        private const string ApiUrl = "https://maps.googleapis.com/maps/api/geocode/json";

        /// <summary>
        /// Communicates with Google Maps API to get location data
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<LocationData> GetLocation(double latitude, double longitude, string apiKey)
        {

            string url = $"{ApiUrl}?latlng={latitude},{longitude}&key={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                // Get Location
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // Convert location to LocationResponse object
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    LocationResponse geocodeResponse = JsonConvert.DeserializeObject<LocationResponse>(jsonResponse);

                    // Get City and State
                    if (geocodeResponse.Status == "OK" && geocodeResponse.Results.Length > 0)
                    {
                        return FilterLocation(geocodeResponse.Results);
                    }
                }
                throw new Exception();
            }
        }

        /// <summary>
        /// Filters data to return city and state names
        /// </summary>
        /// <param name="results"></param>
        /// <returns>City, State</returns>
        private LocationData FilterLocation(LocationResults[] results)
        {
            string city = string.Empty;
            string state = string.Empty;

            foreach (LocationResults result in results)
            {
                foreach(var data in result.AddressComponents)
                {
                    // City Name
                    if (data.Types.Contains("locality"))
                    {
                        city = data.LongName;
                    }

                    // State Name
                    if (data.Types.Contains("administrative_area_level_1"))
                    {
                        state = data.LongName;
                    }
                }
                if (!string.IsNullOrEmpty(city) && !string.IsNullOrEmpty(state))
                {
                    break;
                }
            }
            return new LocationData
            {
                City = city,
                State = state,
            };
        }

        public class LocationData 
        {
            private string _city;
            private string _state;

            public string City { get { return _city; } set { _city = value; } }
            public string State { get { return _state; } set { _state = value; } }
        }

        public class LocationResponse
        {
            private string _status;
            private LocationResults[] _results;

            [JsonProperty("status")]
            public string Status { get { return _status; } set { _status = value; } }

            [JsonProperty("results")]
            public LocationResults[] Results { get { return _results; } set { _results = value; } }
        }

        public class LocationResults
        {
            private AddressComponent[] _addressComponents;

            [JsonProperty("address_components")]
            public AddressComponent[] AddressComponents { get { return _addressComponents; } set { _addressComponents = value; } }
        }

        public class AddressComponent
        {
            private string _longName;
            private string _shortName;
            private string[] _types;

            [JsonProperty("long_name")]
            public string LongName { get { return _longName; } set { _longName = value; } }
            [JsonProperty("short_name")]
            public string ShortName { get { return _shortName; } set { _shortName = value; } }
            [JsonProperty("types")]
            public string[] Types { get { return _types; } set { _types = value; } }
        }
    }
}
