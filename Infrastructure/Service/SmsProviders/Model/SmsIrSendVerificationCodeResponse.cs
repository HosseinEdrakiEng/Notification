using System.Text.Json.Serialization;

namespace Infrastructure.Service.SmsProviders.Model
{
    public class SmsIrSendVerificationCodeResponse
    {
        [JsonPropertyName("data")]
        public SmsIrSendVerificationCodeResponseData Data { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
    public class SmsIrSendVerificationCodeResponseData
    {
        [JsonPropertyName("messageId")]
        public long MessageId { get; set; }

        [JsonPropertyName("cost")]
        public float Cost { get; set; }

    }
}

//{ 
//    "data":
//        { 
//        "   messageId":164562845,
//            "cost":1.05
//        },
//    "status":1
//    ,"message":"موفق"}