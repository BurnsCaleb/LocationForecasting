using Microsoft.Data.SqlClient;

namespace LocationForecasting.Data
{
    public class ExceptionData : DatabaseAccess
    {
        public ExceptionData(IConfiguration configuration) : base(configuration) { }

        // Create Exception Log
        public void LogException(Exception exception)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand("INSERT INTO ExceptionLogs (ExceptionMessage, StackTrace) VALUES (@Message, @StackTrace)", connection))
                {
                    cmd.Parameters.AddWithValue("@Message", exception.Message);
                    cmd.Parameters.AddWithValue("@StackTrace", exception.StackTrace);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
