using SSLValidator.Shared;
using Microsoft.AspNetCore.Mvc;

namespace SSLValidator.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AssessedSSLCertificateController : ControllerBase
	{
		private readonly ILogger<AssessedSSLCertificateController> _logger;

		public AssessedSSLCertificateController(ILogger<AssessedSSLCertificateController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public IEnumerable<AssessedSSLCertification> Get()
		{
			return Enumerable.Range(1, 10)
				.Select(index => new AssessedSSLCertification(Random.Shared.Next(1, 55)))
				.ToArray();
		}
	}
}
