using ResourceAllocation.API.Helpers;
using ResourceAllocation.API.Models;

namespace ResourceAllocation.API.Services
{
    public class RecommendationService
    {
        public List<EmployeeRecommendation> RecommendEmployees(
            List<Employee> employees,
            Project project)
        {
            var recommendations = new List<EmployeeRecommendation>();

            foreach (var e in employees)
            {
                if (!e.IsAvailable)
                    continue;

                if (e.Experience < project.MinimumExperience)
                    continue;

                if (e.CurrentAllocationPercent >= 100)
                    continue;

                double skillMatch =
                    SkillHelper.CalculateSkillMatchPercentage(
                        e.Skills,
                        project.RequiredSkills);

                if (skillMatch <= 0)
                    continue;

                // Base score
                double score = e.PerformanceScore;

                // Skill match bonus
                score += skillMatch * 0.3;

                // Experience bonus
                score += e.Experience * 0.5;

                // Workload penalty
                score -= e.CurrentAllocationPercent * 0.2;

                var reason =
                    $"Skill Match: {skillMatch:F0}%, " +
                    $"Performance: {e.PerformanceScore}, " +
                    $"Allocation: {e.CurrentAllocationPercent}%";

                recommendations.Add(new EmployeeRecommendation
                {
                    EmployeeId = e.Id,
                    EmployeeName = e.Name,
                    FinalScore = Math.Round(score, 2),
                    CurrentAllocationPercent =
                        e.CurrentAllocationPercent,
                    Reason = reason
                });
            }

            return recommendations
                .OrderByDescending(r => r.FinalScore)
                .ToList();
        }
    }
}