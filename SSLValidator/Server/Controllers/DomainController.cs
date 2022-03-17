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
		}

		[HttpGet]
		public IEnumerable<Domain> Get()
		{
			return Enumerable.Range(1, 10)
				.Select(index => new Domain("", "", Random.Shared.Next(1, 55)))
				.ToArray();
		}
	}
}
