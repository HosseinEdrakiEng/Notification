using Application.Common;
using Application.Model;

namespace Infrastructure.Service.SmsProviders
{
    public abstract class SmsBaseService
    {
        protected SmsBaseService()
        {

        }

        public async Task<BaseResponse<SendSmsResponseDto>> Send(SendSmsRequestDto request, CancellationToken cancellationToken)
        {
            return await this.SendCore(request, cancellationToken);
        }

        protected abstract SmsProviderType SmsProviderType { get; }
        protected abstract Task<BaseResponse<SendSmsResponseDto>> SendCore(SendSmsRequestDto request, CancellationToken cancellationToken);
    }
}
