namespace Application.Config
{
    public class SmsIrConfig
    {
        public string BaseUrl { get; set; }
        public TimeSpan Timeout { get; set; }
        public string ApiKey { get; set; }
        public string SendUrl { get; set; }
    }
}
