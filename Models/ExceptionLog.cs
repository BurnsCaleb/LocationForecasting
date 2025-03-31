using System.ComponentModel.DataAnnotations;

namespace LocationForecasting.Models
{
    public class ExceptionLog
    {
        private int _settingsId;
        private string _userId;
        private string _exceptionMessage;
        private string _stackTrace;
        private DateTime _exceptionTime = DateTime.Now;

        [Key]
        public int SettingsId { get { return _settingsId; } set { _settingsId = value; } }
        public string UserId { get { return _userId; } set { _userId = value; } }
        public string ExceptionMessage { get { return _exceptionMessage; } set { _exceptionMessage = value; } }
        public string StackTrace { get { return _stackTrace; } set { _stackTrace = value; } }
        public DateTime ExceptionTime { get { return _exceptionTime; } set { _exceptionTime = value; } }
    }
}
