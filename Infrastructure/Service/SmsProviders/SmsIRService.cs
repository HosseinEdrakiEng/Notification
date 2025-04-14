using Application.Abstraction.IService;
using Application.Common;
using Application.Config;
using Application.Model;
using Infrastructure.Service.SmsProviders.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.Service.SmsProviders
{
    public class SmsIRService : SmsBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly ILogger<SmsIRService> _logger;
        private readonly SmsIrConfig _config;

        public SmsIRService(IHttpClientFactory httpClientFactory
            , IOptions<SmsIrConfig> config
            , IBackgroundTaskQueue backgroundTaskQueue
            , ILogger<SmsIRService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _config = config.Value;
            _backgroundTaskQueue = backgroundTaskQueue;
            _logger = logger;
        }

        protected override SmsProviderType SmsProviderType => SmsProviderType.SmsIR;

        protected override async Task<BaseResponse<SendSmsResponseDto>> SendCore(SendSmsRequestDto request, CancellationToken cancellationToken)
        {
            var result = new BaseResponse<SendSmsResponseDto>();

            var val = new
            {
                Mobile = request.PhoneNumber,
                TemplateId = int.Parse(request.TemplateId),
                Parameters = Enumerable.Select(request.Parameters, (KeyValuePair<string, string> item) => new
                {
                    Name = item.Key.ToUpper(),
                    Value = item.Value
                }).ToArray()
            };

            var headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json"},
                { "Accept", "application/json"},
                { "X-API-KEY", _config.ApiKey},
            };

            var response = await _httpClientFactory.ApiCall(this.SmsProviderType.ToString(), val, HttpMethod.Post, _config.SendUrl, headers, cancellationToken);

            _ = _backgroundTaskQueue.EnqueueAsync(async (token) =>
            {
                _logger.LogInformation($"api call log : '{response.SerializeAsJson()}'");
            });

            if (!response.IsSuccessStatusCode || response.Response is null)
            {
                result.Error = Error.SendSmsIrFail;
                return result;
            }

            var model = JsonSerializer.Deserialize<SmsIrSendVerificationCodeResponse>(response.Response);
            if (model is null || model.Data is null || model?.Status != 1)
            {
                result.Error = Error.SendSmsIrFail;
                return result;
            }
            result.Data = new SendSmsResponseDto { ReferenceNo = model.Data?.MessageId.ToString() };
            return result;
        }
    }
}
