using SSLValidator.Shared;
using Microsoft.AspNetCore.Mvc;

namespace SSLValidator.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class DomainController : ControllerBase
	{
		private readonly ILogger<DomainController> _logger;

		public DomainController(ILogger<DomainController> logger)
		{
			_logger = logger;
		[HttpGet("{sessionId}")]
		public async Task<ActionResult<List<Domain>>> Get(string sessionId)
		{
			var domains = new List<Domain>();
			if (string.IsNullOrWhiteSpace(sessionId))
			{
				return BadRequest("Session Id wasn't provided");
			}
			else
			{
				domains = await _cache.GetRecordAsync<List<Domain>>(sessionId + "-domains");
			}

			return Ok(domains);
		}

		[HttpGet]
		public IEnumerable<Domain> Get()
		public static string RandomString()
		{
			var random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, 34)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}
}
