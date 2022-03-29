using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;

using SSLValidator.Shared;
using SSLValidator.Server.Hubs;
using SSLValidator.Server.Extensions;

using System.Text.Json;

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
				var parsedRedisConnectionString = _redisConnection.Substring(0, _redisConnection.IndexOf(","));
				var splitRedisConnectionString = parsedRedisConnectionString.Split(":");

				if (!int.TryParse(splitRedisConnectionString[1], out var redisConnectionStringPort)) throw new Exception("Redis connection string invalid");

				var keys = m.GetServer(splitRedisConnectionString[0], redisConnectionStringPort).Keys().ToList();
				var domains = new List<Domain>();

				if (keys.Any())
				{
					var parsedKeys = keys.Select(key => key.ToString().Replace("sslValidator_", string.Empty));
					var cachedDomains = (await Task.WhenAll<byte[]>(parsedKeys.Select(parsedKey => _cache.GetAsync(parsedKey)))).ToList();
					var cachedDomainsWithKey = parsedKeys.Select((key, index) => (key, cachedDomains[index])).ToList();

					cachedDomainsWithKey.ForEach(domain =>
					{
						var parsedDomains = JsonSerializer.Deserialize<List<Domain>>(domain.Item2);
						if (parsedDomains is not null)
						{
							domains.AddRange(parsedDomains);
						}
					});

					await Task.WhenAll(parsedKeys.Select((key, index) => _cache.SetRecordAsync(key, domains[index])));

					await _domainHubContext.Clients.All.SendAsync("ReceiveCurrentDomains", domains);
				}
			}
			catch (Exception)
			{
				throw new Exception("Something went wrong running background service to update domains");
			}
		}
	}
}
