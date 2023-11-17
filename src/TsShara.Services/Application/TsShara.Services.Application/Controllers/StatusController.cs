using Microsoft.AspNetCore.Mvc;
using TsShara.Services.Domain.Interfaces;

namespace TsShara.Services.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly ILogger<StatusController> _logger;
        private readonly ITsSharaStatusFromUsb _serialUsbReader;

        public StatusController(ILogger<StatusController> logger, ITsSharaStatusFromUsb serialUsbReader)
        {
            _logger = logger;
            _serialUsbReader = serialUsbReader;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            try
            {
                var data = await _serialUsbReader.GetAsync(cancellationToken);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
