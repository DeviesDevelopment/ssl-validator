namespace SSLValidator.Shared
{
    public class AssessedSSLCertification
    {
        public AssessedSSLCertification(int daysUntilExpiration)
        {
            DaysUntilExpiration = daysUntilExpiration;
            var fourWorkingWeeks = 20; // 4 working weeks
            var twoWorkingWeeks = 10; // 2 working weeks
            if (daysUntilExpiration >= fourWorkingWeeks)
            {
                ThreatLevel = ThreatLevel.Low;
            }
            else if(daysUntilExpiration > twoWorkingWeeks && daysUntilExpiration <= fourWorkingWeeks)
            {
                ThreatLevel = ThreatLevel.Medium;
            }
            else
            {
                ThreatLevel = ThreatLevel.High;
            }
        }

        public int DaysUntilExpiration { get; set; }

        public ThreatLevel ThreatLevel { get; set; }

        public string ThreatLevelClass
        {
            get
            {
                var baseThreatLevelClass = "threat__";
                return ThreatLevel switch
                {
                    ThreatLevel.High => baseThreatLevelClass + "high",
                    ThreatLevel.Medium => baseThreatLevelClass + "medium",
                    ThreatLevel.Low => baseThreatLevelClass + "low",
                    _ => string.Empty
                };
            }
        }
    }

    public enum ThreatLevel
    {
        High,
        Medium,
        Low,
    }
}
