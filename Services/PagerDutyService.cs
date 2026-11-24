using System.Text;
using System.Text.Json;

namespace PagerDutySample.Services;

public class PagerDutyService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public PagerDutyService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task TriggerIncidentAsync(Exception ex)
    {
        var payload = new
        {
            routing_key = _configuration["PagerDuty:RoutingKey"],
            event_action = "trigger",
            payload = new
            {
                summary = ex.Message,
                source = Environment.MachineName,
                severity = "error",
                component = "PagerDutySample",
                group = "Backend",
                custom_details = new
                {
                    StackTrace = ex.StackTrace
                }
            }
        };

        var json = JsonSerializer.Serialize(payload);

        await _httpClient.PostAsync(
            "https://events.pagerduty.com/v2/enqueue",
            new StringContent(
                json,
                Encoding.UTF8,
                "application/json"));
    }
}
