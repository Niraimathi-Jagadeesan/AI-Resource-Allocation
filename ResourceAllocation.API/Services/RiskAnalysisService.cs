using System.Text.Json;
using ResourceAllocation.API.DTOs;
using ResourceAllocation.API.Models;

namespace ResourceAllocation.API.Services
{
    public class RiskAnalysisService
    {
        private readonly ILLMService _llmService;

        public RiskAnalysisService(ILLMService llmService)
        {
            _llmService = llmService;
        }

        public async Task<RiskAnalysisResponse> AnalyzeAsync(Project project)
        {
            var prompt = $@"
                You are a senior project risk analysis expert.

                Analyze the following project and return ONLY valid JSON.

                Project Details:
                - Project Name: {project.ProjectName}
                - Required Skills: {project.RequiredSkills}
                - Minimum Experience: {project.MinimumExperience}
                - Priority: {project.Priority}
                - Complexity: {project.Complexity}

                Return JSON in this exact format:

                {{
                  ""riskScore"": 75,
                  ""riskLevel"": ""High"",
                  ""summary"": ""This project has a high risk due to complexity and skill requirements."",
                  ""riskFactors"": [
                    ""High project complexity."",
                    ""Requires specialized technical skills.""
                  ],
                  ""recommendations"": [
                    ""Assign senior engineers."",
                    ""Conduct weekly risk reviews.""
                  ]
                }}
                ";

            try
            {
                var aiResponse = await _llmService.AskAsync(prompt);

                // Remove markdown code fences if the model includes them.
                aiResponse = aiResponse
                    .Replace("```json", "")
                    .Replace("```", "")
                    .Trim();

                var result = JsonSerializer.Deserialize<RiskAnalysisResponse>(
                    aiResponse,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (result != null)
                    return result;
            }
            catch
            {
                // Ignore and use fallback response below.
            }

            // Fallback response
            return new RiskAnalysisResponse
            {
                RiskScore = 50,
                RiskLevel = "Medium",
                Summary =
                    "AI service is unavailable. Returning default risk assessment.",
                RiskFactors = new List<string>
                {
                    "Unable to generate AI-based risk factors."
                },
                Recommendations = new List<string>
                {
                    "Review project details manually."
                }
            };
        }
    }
}