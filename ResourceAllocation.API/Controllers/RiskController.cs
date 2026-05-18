using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceAllocation.API.Data;
using ResourceAllocation.API.Services;

namespace ResourceAllocation.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RiskController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly RiskAnalysisService _riskAnalysisService;

        public RiskController(
            AppDbContext context,
            RiskAnalysisService riskAnalysisService)
        {
            _context = context;
            _riskAnalysisService = riskAnalysisService;
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> Get(int projectId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                return NotFound("Project not found.");

            var result =
                await _riskAnalysisService.AnalyzeAsync(project);

            return Ok(result);
        }
    }
}