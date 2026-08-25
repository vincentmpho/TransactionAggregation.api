using Microsoft.AspNetCore.Mvc;

namespace TransactionAggregation.API.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    // A simple endpoint monitoring use to check the API is running.
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "healthy" });
}