using Microsoft.AspNetCore.Mvc;
using PagerDutySample.Services;

namespace PagerDutySample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly PagerDutyService _pagerDutyService;

    public TestController(
        PagerDutyService pagerDutyService)
    {
        _pagerDutyService = pagerDutyService;
    }

    [HttpGet("generate-error")]
    public async Task<IActionResult> GenerateError()
    {
        try
        {
            throw new NullReferenceException(
                "Test exception for PagerDuty");
        }
        catch (Exception ex)
        {
            await _pagerDutyService.TriggerIncidentAsync(ex);

            return StatusCode(
                500,
                new
                {
                    Message = "Error sent to PagerDuty"
                });
        }
    }
}
