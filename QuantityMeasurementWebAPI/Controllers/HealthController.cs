using Microsoft.AspNetCore.Mvc;

namespace QuantityMeasurementWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                status = "Healthy",
                timestamp = DateTime.UtcNow,
                version = "1.0.0"
            });
        }

        [HttpGet("ready")]
        public IActionResult Ready()
        {
            // Add any readiness checks here (database connectivity, etc.)
            return Ok(new
            {
                status = "Ready",
                timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("live")]
        public IActionResult Live()
        {
            // Add any liveness checks here
            return Ok(new
            {
                status = "Alive",
                timestamp = DateTime.UtcNow
            });
        }
    }
}
