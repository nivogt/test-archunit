using Microsoft.AspNetCore.Mvc;

namespace TestArchUnit.API.Controllers;

/// <summary>
/// Sample Health check controller for monitoring API health.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Get the health status of the API
    /// </summary>
    [HttpGet]
    public ActionResult<HealthResponse> GetHealth()
    {
        return Ok(new HealthResponse
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Version = "1.0.0"
        });
    }
}

/// <summary>
/// Health status response model
/// </summary>
public class HealthResponse
{
    public string Status { get; set; } = "Unknown";
    public DateTime Timestamp { get; set; }
    public string Version { get; set; } = string.Empty;
}
