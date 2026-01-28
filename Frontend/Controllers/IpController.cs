using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IpController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetClientIp()
        {
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";

            if (ip == "::1")
            {
                return Ok("127.0.0.1");
            }

            return Ok(ip);
        }
    }
}
