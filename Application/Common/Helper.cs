using System.Net;
using System.Text;
using System.Text.Json;

namespace Application.Common
{
    public class ApiResponse
    {
        public string Request { get; set; }
        public string Response { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public Exception Exception { get; set; }
        public string Url { get; set; }
        public bool IsSuccessStatusCode { get; set; }
    }
    public static class Helper
    {
        public static async Task<ApiResponse> ApiCall<T>(this IHttpClientFactory httpClientFactory
            , string name
            , T model
            , HttpMethod httpMethod
            , string url
            , Dictionary<string, string> headers
            , CancellationToken cancellationToken)
        {
            var result = new ApiResponse();
            try
            {
                var requestContent = model.SerializeAsJson();
                result.Request = requestContent;
                result.Url = url;

                var httpClient = httpClientFactory.CreateClient(name);
                var requestMessage = new HttpRequestMessage(httpMethod, url);

                HttpContent content = null;
                string contentTypeValue = string.Empty;
                if (headers.TryGetValue("Content-Type", out contentTypeValue) && contentTypeValue == "application/json")
                {
                    content = new StringContent(requestContent, Encoding.UTF8, "application/json");
                }
                else if (headers.TryGetValue("Content-Type", out contentTypeValue) && contentTypeValue == "application/x-www-form-urlencoded")
                {
                    if (model is Dictionary<string, string> t)
                    {
                        var modelData = t.ToList();
                        content = new FormUrlEncodedContent(modelData);
                    }
                }
                else
                    throw new NotSupportedException();

                requestMessage.Content = content;

                foreach (var header in headers)
                    requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);

                var httpResponse = await httpClient.SendAsync(requestMessage, cancellationToken);
                result.StatusCode = httpResponse.StatusCode;
                result.Response = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
                result.IsSuccessStatusCode = httpResponse.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }
            return result;
        }

        public static string SerializeAsJson(this object model, JsonSerializerOptions option = null)
        {
            return JsonSerializer.Serialize(model, options: option ?? new JsonSerializerOptions());
        }
    }
}
