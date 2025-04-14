using System.Net;
using System.Text.Json.Serialization;

namespace Application.Common
{
    public record class Error(string Code, string Discription, [property: JsonIgnore] HttpStatusCode StatusCode)
    {
        public static readonly Error None = new("00", "Success", HttpStatusCode.OK);
        public static readonly Error SendSmsIrFail = new("01", "Send sms fail.please check api call log", HttpStatusCode.BadRequest);
        public static readonly Error GlobalError = new("-1", "InternalServerError Please try again", HttpStatusCode.InternalServerError);
        public static readonly Error SmsProviderNotFound = new("02", "Sms provider not found", HttpStatusCode.BadRequest);
    }
}
