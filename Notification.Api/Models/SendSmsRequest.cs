using System.ComponentModel.DataAnnotations;

namespace Notification.Api.Models
{
    public class SendSmsRequest
    {
        [Required]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "Phone number must start with '09' and be 11 characters")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "text must 1 to 500 characters")]
        public string Text { get; set; }

        public string TemplateId { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
