using System.ComponentModel.DataAnnotations;

namespace LocationForecasting.Models
{
    public class KeyStorage
    {
        private string _key = string.Empty;

        [Key]
        public string Key { get { return _key; } set { _key = value; } }
    }
}
