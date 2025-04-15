using System.Text.Json;
using LocationForecasting.Models;
using Microsoft.Data.SqlClient;

namespace LocationForecasting.Data
{
    public class RequestData : DatabaseAccess
    {
        public RequestData(IConfiguration configuration) : base(configuration) { }

        // Create Log
        public void LogRequest(WeatherService.WeatherRequest[] data)
        {
            var jsonData = JsonSerializer.Serialize(data);

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("INSERT INTO RequestLogs (RequestPayload) VALUES (@Data)", connection))
                {
                    // Add @Data Parameter
                    cmd.Parameters.AddWithValue("@Data", jsonData);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
