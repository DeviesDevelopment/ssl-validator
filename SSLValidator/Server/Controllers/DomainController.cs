using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

using SSLValidator.Shared;
using SSLValidator.Server.Hubs;
using SSLValidator.Server.Services;
using SSLValidator.Server.Extensions;

namespace SSLValidator.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class DomainController : ControllerBase
	{
		private readonly ILogger<DomainController> _logger;

		private readonly IDistributedCache _cache;

		private readonly IHubContext<DomainHub> _domainHubContext;

		public DomainController(ILogger<DomainController> logger, IDistributedCache cache, IHubContext<DomainHub> domainHubContext)
		{
			_logger = logger;
			_cache = cache;
			_domainHubContext = domainHubContext;
		}

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

		[HttpGet("get-session-id")]
		public ActionResult<string> GetSessionId()
		{
			return Ok(RandomString());
		}

		[HttpPost("{sessionId}")]
		public async Task<ActionResult> Post(string sessionId, [FromBody] DomainPayload payload)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			var isURLValid = payload.URL.Contains("https");

			if (!isURLValid)
			{
				return BadRequest("Wrong URL format supplied, use https:// at start of the url");
			}

			if (string.IsNullOrWhiteSpace(sessionId))
			{
				return BadRequest("No session id");
			}

			var domains = await _cache.GetRecordAsync<List<Domain>>(sessionId + "-domains");

			if (domains is null)
			{
				domains = new List<Domain>();
			}

			var expirationDate = await GetSSLCertificateExpirationDate.GetDaysUntilExpirationAsync(payload.URL);
			var newDomain = new Domain(payload.DomainName, payload.URL, expirationDate);

			domains.Add(newDomain);
			await _cache.SetRecordAsync(sessionId + "-domains", domains);

			await _domainHubContext.Clients.All.SendAsync("ReceiveCurrentDomains", domains);

			return Ok();
		}

		public static string RandomString()
		{
			var random = new Random();
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, 34)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}
}
