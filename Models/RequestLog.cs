using System.ComponentModel.DataAnnotations;

namespace LocationForecasting.Models
{
    public class RequestLog
    {
        private int _requestId;
        private string _userId;
        private double _latitude;
        private double _longitude;
        private string _requestUrl;
        private string _responseStatus;
        private string _responseData;
        private DateTime _requestTime = DateTime.Now;

        [Key]
        public int RequestId { get { return _requestId; } set { _requestId = value; } }
        public string UserId { get { return _userId; } set { _userId = value; } }
        public double Latitude { get { return _latitude; } set { _latitude = value; } }
        public double Longitude { get { return _longitude; } set { _longitude = value; } }
        public string RequestUrl { get { return _requestUrl; } set { _requestUrl = value; } }
        public string ResponseStatus { get { return _responseStatus; } set { _responseStatus = value; } }
        public string ResponseData { get { return _responseData; } set { _responseData = value; } }
        public DateTime RequestTime { get { return _requestTime; } set { _requestTime = value; } }
    }
}
