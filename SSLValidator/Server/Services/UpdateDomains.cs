using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;

using SSLValidator.Shared;
using SSLValidator.Server.Hubs;
using SSLValidator.Server.Extensions;

namespace SSLValidator.Server.Services
{
	public class UpdateDomains : BackgroundService
	{
		private readonly int _serviceIntervalMinutes;

		private readonly IServiceProvider _serviceProvider;

		private readonly IDistributedCache _cache;

		private readonly IHubContext<DomainHub> _domainHubContext;

		private readonly string _redisConnection;

		public UpdateDomains(IConfiguration config, IDistributedCache cache, IHubContext<DomainHub> domainHubContext)
		{
			_serviceIntervalMinutes = config.GetValue<int?>("checkDomainsIntervalMinutes") ?? 1440; // defaults to every 24 hours
			_cache = cache;
			_domainHubContext = domainHubContext;
			_redisConnection = config.GetConnectionString("redis");
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				await UpdateDomainsFunc();
				await Task.Delay(_serviceIntervalMinutes * 60 * 1000, stoppingToken);
			}
		}

		private async Task UpdateDomainsFunc()
		{
			try
			{
				var m = await StackExchange.Redis.ConnectionMultiplexer.ConnectAsync(_redisConnection);
				var keys = m.GetServer("localhost", 5003).Keys().ToList();
				var domains = new List<Domain>();
				if (keys.Any())
				{
					keys.ForEach(async (key) =>
					{
						domains.Add(await _cache.GetRecordAsync<Domain>(key));
					});
					domains.ForEach(async (domain) =>
					{
						domain.DaysUntilExpiration = await GetSSLCertificateExpirationDate.GetDaysUntilExpirationAsync(domain.Url);
					});
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
