namespace ResourceAllocation.API.Models
{
    public class RiskResult
    {
        public string RiskLevel { get; set; } = "Low";

        public double Probability { get; set; }

        public string Summary { get; set; } = string.Empty;
    }
}