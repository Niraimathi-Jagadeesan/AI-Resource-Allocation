namespace ResourceAllocation.API.DTOs
{
    public class RiskAnalysisResponse
    {
        public double RiskScore { get; set; }
        public string RiskLevel { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public List<string> RiskFactors { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
    }
}