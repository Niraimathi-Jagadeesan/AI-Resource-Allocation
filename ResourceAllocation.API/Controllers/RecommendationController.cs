using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceAllocation.API.Data;
using ResourceAllocation.API.Models;
using ResourceAllocation.API.Services;

namespace ResourceAllocation.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly RecommendationService _recommendationService;
        private readonly ILLMService _llmService;

        public RecommendationController(
            AppDbContext context,
            RecommendationService recommendationService,
            ILLMService llmService)
        {
            _context = context;
            _recommendationService = recommendationService;
            _llmService = llmService;
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<List<EmployeeRecommendation>>> Get(int projectId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                return NotFound("Project not found.");

            var employees = await _context.Employees.ToListAsync();

            // Step 1: Get only suitable employees
            var recommendations = _recommendationService
                .RecommendEmployees(employees, project);

            if (!recommendations.Any())
                return Ok(new List<EmployeeRecommendation>());

            // Step 2: Generate AI explanation for EACH suitable employee
            foreach (var recommendation in recommendations)
            {
                var prompt = $"""
                    You are an AI resource allocation assistant.

                    Explain in 2-3 professional sentences why
                    {recommendation.EmployeeName} is suitable for the project
                    "{project.ProjectName}".

                    Metrics:
                    {recommendation.Reason}
                    Final Score: {recommendation.FinalScore}
                    Current Allocation: {recommendation.CurrentAllocationPercent}%
                    """;

                try
                {
                    recommendation.Reason =
                        await _llmService.AskAsync(prompt);
                }
                catch
                {
                    // Rebuild the original rule-based explanation if the LLM call fails.
                    recommendation.Reason =
                        $"Employee {recommendation.EmployeeName} is suitable for " +
                        $"the project because of a strong overall score of " +
                        $"{recommendation.FinalScore:F2}. " +
                        $"Current allocation is " +
                        $"{recommendation.CurrentAllocationPercent}%, " +
                        $"which indicates available capacity.";
                }
            }

            // Step 3: Return only the top N candidates if desired
            // Uncomment the next line to show only the top 5.
            recommendations = recommendations.Take(5).ToList();

            return Ok(recommendations);
        }
    }
}