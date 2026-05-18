namespace ResourceAllocation.API.Models
{
    public class EmployeeRecommendation
    {
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; } = string.Empty;

        public double FinalScore { get; set; }

        public int CurrentAllocationPercent { get; set; }

        public string Reason { get; set; } = string.Empty;
    }
}