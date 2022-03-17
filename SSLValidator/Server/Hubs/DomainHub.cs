using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;

using SSLValidator.Shared;
using SSLValidator.Server.Services;
using SSLValidator.Server.Extensions;

using System.Text.Json;

namespace SSLValidator.Server.Hubs
{
	public class DomainHub : Hub
	{
		private readonly IDistributedCache _cache;

		public DomainHub(IDistributedCache cache)
		{
			_cache = cache;
		}

		public async Task GetCurrentDomains(string sessionId)
		{
			var getDaysUntilExpiration = await GetSSLCertificateExpirationDate.GetDaysUntilExpirationAsync("https://castello.smwg.se");
			var domains = await _cache.GetRecordAsync<List<Domain>>("domains");
			if (domains is null)
			{
				domains = new List<Domain>();
				await _cache.SetRecordAsync($"domains-{sessionId}", domains);
			}
			await Clients.Caller.SendAsync("ReceiveCurrentDomains", domains);
		}
	}
}
