namespace PlanCorp.Models
{
    public class ResponseJson
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string UrlResponse { get; set; }
    }

    public class BaseResponseJson
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}
