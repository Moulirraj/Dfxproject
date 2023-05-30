namespace PaymentAPI.Model
{
    public class Response
    {
        public ServiceStatus Status { get; set; }
        public string Error { get; set; }
        public int StatusCode { get; set; }
        public string data { get; set; }
    }

    public enum ServiceStatus
    {
        Success,
        DataError,
        InternalError
    }
    public enum prospectType
    {
        Personal,
        Corporate
    }
}
