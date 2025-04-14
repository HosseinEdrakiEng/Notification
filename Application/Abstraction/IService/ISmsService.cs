using Application.Common;
using Application.Model;

namespace Application.Abstraction.IService
{
    public interface ISmsService
    {
        Task<BaseResponse<SendSmsResponseDto>> Send(SendSmsRequestDto request, CancellationToken cancellationToken);
    }
}
