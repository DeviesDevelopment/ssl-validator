using Microsoft.Extensions.Caching.Distributed;

using System.Text.Json;

namespace SSLValidator.Server.Extensions
{
	public static class DistributedCacheExtension
	{
		public static async Task SetRecordAsync<T>(this IDistributedCache cache, string recordId, T data, TimeSpan? absoluteExpireTime = null)
		{
			var options = new DistributedCacheEntryOptions();
			options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromDays(2);
			options.SlidingExpiration = TimeSpan.FromDays(2);
			var jsonData = JsonSerializer.Serialize(data);
			await cache.SetStringAsync(recordId, jsonData, options);
		}

		public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
		{
			var jsonData = await cache.GetStringAsync(recordId);
			if (string.IsNullOrWhiteSpace(jsonData))
			{
				return default(T);
			}
			return JsonSerializer.Deserialize<T>(jsonData);
		}
	}
}
