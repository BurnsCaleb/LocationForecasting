using Microsoft.Data.SqlClient;

namespace LocationForecasting.Data
{
    public class SettingsData : DatabaseAccess
    {
        public SettingsData(IConfiguration configuration) : base(configuration) { }

        // Create ApiKey
        public void CreateApiKey(string apiKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("INSERT INTO Settings (ApiKey) VALUES (@apiKey)", connection))
                {
                    cmd.Parameters.AddWithValue("@ApiKey", apiKey);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Read ApiKey
        public string GetApiKey()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("SELECT TOP 1 ApiKey FROM Settings", connection))
                {
                    connection.Open();
                    var result = cmd.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
            }
        }

        // Update ApiKey
        public void UpdateApiKey(string apiKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("UPDATE Settings SET ApiKey = @ApiKey", connection))
                {
                    cmd.Parameters.AddWithValue("@ApiKey", apiKey);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
