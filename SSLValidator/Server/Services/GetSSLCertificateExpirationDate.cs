namespace SSLValidator.Server.Services
{
	public static class GetSSLCertificateExpirationDate
	{
		public static async Task<int> GetDaysUntilExpirationAsync(string url)
		{
			var daysUntilExpiration = 0;
			var clientHandler = new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = (_, cert, __, ___) =>
				{
					if (cert is null) return true;

					if (DateTime.TryParse(cert.GetExpirationDateString(), out var expirationDate))
					{
						daysUntilExpiration = (expirationDate - DateTime.Now).Days;
					}
					return true;
				}
			};

			var client = new HttpClient(clientHandler);
			await client.GetAsync(url);
			return daysUntilExpiration;
		}
	}
}
