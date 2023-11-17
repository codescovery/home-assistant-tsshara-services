using Microsoft.AspNetCore.Mvc;
using TsShara.Services.Domain.Interfaces;

namespace TsShara.Services.Application.Controllers;

[ApiController]
[Route("[controller]")]
public class FeatureController : Controller
{
    private readonly IFeatureService _featureService;

    public FeatureController(IFeatureService featureService)
    {
        _featureService = featureService;
    }

    [HttpPut("notifier/start")]
    public async Task<IActionResult> StartNotifier(CancellationToken cancellationToken)
    {
        try
        {
            await _featureService.StartNotifiersAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPut("notifier/stop")]
    public async Task<IActionResult> StopNotifier(CancellationToken cancellationToken)
    {
        try
        {
            await _featureService.StopNotifiersAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPut("console/start")]
    public async Task<IActionResult> StartConsole(CancellationToken cancellationToken)
    {
        try
        {
            await _featureService.StartConsoleMonitoringAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPut("console/stop")]
    public async Task<IActionResult> StopConsole(CancellationToken cancellationToken)
    {
        try
        {
            await _featureService.StopConsoleMonitoringAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}