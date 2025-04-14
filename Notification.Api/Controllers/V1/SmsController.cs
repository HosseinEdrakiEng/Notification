using Application.Abstraction.IService;
using Application.Model;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Notification.Api.Models;

namespace Notification.Api.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class SmsController : ControllerBase
    {
        private readonly ISmsService _smsService;

        public SmsController(ISmsService smsService)
        {
            _smsService = smsService;
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] SendSmsRequest request, CancellationToken cancellationToken)
        {
            var dto = new SendSmsRequestDto
            {
                Text = request.Text,
                PhoneNumber = request.PhoneNumber,
                Parameters = request.Parameters,
                TemplateId = request.TemplateId
            };
            var response = await _smsService.Send(dto, cancellationToken);
            return StatusCode((int)response.Error.StatusCode, response);
        }
    }
}
