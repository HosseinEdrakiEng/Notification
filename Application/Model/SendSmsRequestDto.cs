namespace Application.Model
{
    public class SendSmsRequestDto
    {
        public string PhoneNumber { get; set; }
        public string Text { get; set; }
        public string TemplateId { get; set; }
        public Dictionary<string, string> Parameters { get;set; }
    }
    public class SendSmsResponseDto
    {
        public string ReferenceNo { get; set; }
    }
}
