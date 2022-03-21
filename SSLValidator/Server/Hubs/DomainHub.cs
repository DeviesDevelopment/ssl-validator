using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;

using SSLValidator.Shared;
using SSLValidator.Server.Extensions;

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
			var domains = await _cache.GetRecordAsync<List<Domain>>(sessionId + "-domains");
			if (domains is null)
			{
				domains = new List<Domain>();
				await _cache.SetRecordAsync($"{sessionId}-domains", domains);
			}
			await Clients.Caller.SendAsync("ReceiveCurrentDomains", domains);
		}
	}
}
