namespace SSLValidator.Shared
{
	public class Domain
	{
		public Domain(string domainName, string url, int daysUntilExpiration)
		{
			DomainName = domainName;
			Url = url;
			DaysUntilExpiration = daysUntilExpiration;
			var fourWorkingWeeks = 20; // 4 working weeks
			var twoWorkingWeeks = 10; // 2 working weeks
			if (daysUntilExpiration >= fourWorkingWeeks)
			{
				ThreatLevel = DomainExpirationThreatLevel.Low;
			}
			else if (daysUntilExpiration > twoWorkingWeeks && daysUntilExpiration <= fourWorkingWeeks)
			{
				ThreatLevel = DomainExpirationThreatLevel.Medium;
			}
			else
			{
				ThreatLevel = DomainExpirationThreatLevel.High;
			}
		}

		public string DomainName { get; set; }

		public string Url { get; set; }

		public int DaysUntilExpiration { get; set; }

		public DomainExpirationThreatLevel ThreatLevel { get; set; }

		public string ThreatLevelClass
		{
			get
			{
				var baseThreatLevelClass = "threat__";
				return ThreatLevel switch
				{
					DomainExpirationThreatLevel.High => baseThreatLevelClass + "high",
					DomainExpirationThreatLevel.Medium => baseThreatLevelClass + "medium",
					DomainExpirationThreatLevel.Low => baseThreatLevelClass + "low",
					_ => string.Empty
				};
			}
		}
	}

	public enum DomainExpirationThreatLevel
	{
		High,
		Medium,
		Low,
	}
}
