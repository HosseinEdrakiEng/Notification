using Application.Abstraction.IService;
using Application.Common;
using Application.Model;
using Infrastructure.Service.SmsProviders;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Service
{
    public class SmsService : ISmsService
    {
        private readonly IServiceProvider _serviceProvider;

        public SmsService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<BaseResponse<SendSmsResponseDto>> Send(SendSmsRequestDto request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<SendSmsResponseDto>();

            var provider = _serviceProvider.GetKeyedService<SmsBaseService>(SmsProviderType.SmsIR);
            if (provider is null)
            {
                response.Error = Error.SmsProviderNotFound;
                return response;
            }
            var result = await provider.Send(request, cancellationToken);
            if (result.HasError)
                response.Error = result.Error;
            else
                response.Data = new SendSmsResponseDto
                {
                    ReferenceNo = result.Data.ReferenceNo
                };

            return response;
        }
    }
}
