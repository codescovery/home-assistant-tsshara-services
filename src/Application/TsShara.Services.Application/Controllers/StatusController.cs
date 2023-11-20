using Codescovery.Library.Commons.Extensions;
using Microsoft.AspNetCore.Mvc;
using TsShara.Services.Application.ViewModels;
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
        public async Task<IActionResult> Get([FromQuery]string? port=null, CancellationToken cancellationToken=default)
        {
            try
            {
                var data = string.IsNullOrWhiteSpace(port)? await _serialUsbReader.GetAsync(cancellationToken)
                        : await _serialUsbReader.GetAsync(port, cancellationToken);
                if (data == null) return NoContent();
                if (data.IsType<ITsSharaInformationError>())
                    return await HandleErrorAsync(data, cancellationToken);

                if (data.IsType<ITsSharaInformationData>())
                    return new ObjectResult(new TsSharaStatusDataResponseModel(data.AsData()!,_serialUsbReader.IsMonitoring, _serialUsbReader.MonitoredPort?.PortName??string.Empty));
                return data.IsType<ITsSharaInformationDataRaw>() ? new ObjectResult(new TsSharaStatusRawResponseModel(data.AsDataRaw()!,_serialUsbReader.IsMonitoring, _serialUsbReader.MonitoredPort?.PortName??string.Empty)) : new ObjectResult(new TsSharaStatusResponseModel(_serialUsbReader.IsMonitoring, _serialUsbReader.MonitoredPort?.PortName??string.Empty));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        

        private async Task<IActionResult> HandleErrorAsync(ITsSharaInformationResult data, CancellationToken cancellationToken)
        {
            var error = data.AsError();
            return await Task.FromResult(error == null
                ? Problem("Unhandled error", statusCode: StatusCodes.Status500InternalServerError)
                : Problem(error.Exception.GetFullMessage(), error.Exception.Source,
                    StatusCodes.Status500InternalServerError,
                    error.Exception.Message, error.Exception.GetType().FullName));
        }
    }
}
