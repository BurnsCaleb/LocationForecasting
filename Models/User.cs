using System.ComponentModel.DataAnnotations;

namespace LocationForecasting.Models
{
    public class User
    {
        private string _userId;

        [Key]
        public string UserId { get { return _userId; } set { _userId = value; } }
    }
}
