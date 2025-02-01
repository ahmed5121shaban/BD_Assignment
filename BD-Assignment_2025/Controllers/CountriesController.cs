using BD_Services;
using Microsoft.AspNetCore.Mvc;

namespace BD_Assignment_2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly BlockedCountriesService _service;

        public CountriesController(BlockedCountriesService service)
        {
            _service = service;
        }

        [HttpPost("block")]
        public IActionResult BlockCountry([FromBody] string ip)
        {
            var result = _service.AddBlockedCountry(ip);
            return result ? Ok() : BadRequest("Country already blocked.");
        }

        [HttpDelete("block/{countryCode:alpha}")]
        public IActionResult UnblockCountry(string countryCode)
        {
            var result = _service.RemoveBlockedCountry(countryCode);
            return result ? NoContent() : NotFound();
        }

        [HttpGet("blocked")]
        public IActionResult GetAllBlockedCountries([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string filter = null)
        {
            var countries = _service.GetAllBlockedCountries(page, pageSize, filter);
            return Ok(countries);
        }

        [HttpGet("ip/lookup")]
        public async Task<IActionResult> LookupIp([FromQuery]string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            }

            var country = await _service.FindMyCountry(ipAddress);
            return Ok(country);
        }

        [HttpGet("ip/check-block")]
        public async Task<IActionResult> CheckBlock()
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var isBlocked = _service.VerifyBlockedCountry(ipAddress);

            var country = await _service.FindMyCountry(ipAddress);
            var userAgent = HttpContext.Request.Headers.UserAgent.ToString();
            _service.LogBlockedAttempt(ipAddress, country.Country, isBlocked, userAgent);

            return Ok(new { IsBlocked = isBlocked });
        }

        [HttpGet("logs/blocked-attempts")]
        public IActionResult GetBlockedAttempts([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string filter = null)
        {
            var logs = _service.GetAllBlockedAttempts(page, pageSize, filter);
            return Ok(logs);
        }


        [HttpPost("temporal-block")]
        public IActionResult TemporarilyBlockCountry(string CountryCode, int DurationMinutes)
        {
            if (DurationMinutes < 1 || DurationMinutes > 1440)
                return BadRequest("Duration must be between 1 and 1440 minutes.");

            if (string.IsNullOrEmpty(CountryCode) || CountryCode.Length != 2)
                return BadRequest("Invalid country code.");

            int checkedCountry = _service.AddTemporalBlock(CountryCode, DurationMinutes);
            if (checkedCountry == 0)
                return Conflict("Country is already temporarily blocked.");

            return checkedCountry == 2 ? Ok() : StatusCode(500, "Failed to add temporary block.");
        }
    }
}
   


