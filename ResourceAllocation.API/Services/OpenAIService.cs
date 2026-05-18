using OpenAI;
using OpenAI.Chat;

namespace ResourceAllocation.API.Services
{
    public class OpenAIService
    {
        private readonly ChatClient _chatClient;

        public OpenAIService(IConfiguration configuration)
        {
            var apiKey = configuration["OpenAI:ApiKey"];
            var model = configuration["OpenAI:Model"] ?? "gpt-4.1-mini";

            var client = new OpenAIClient(apiKey);
            _chatClient = client.GetChatClient(model);
        }

        public async Task<string> GenerateRecommendationExplanationAsync(
            string employeeName,
            string projectName,
            string reason)
        {
            var prompt = $"""
            You are an AI resource allocation assistant.

            Explain in 3-4 professional sentences why employee '{employeeName}'
            is recommended for project '{projectName}'.

            Data:
            {reason}
            """;

            var response = await _chatClient.CompleteChatAsync(prompt);

            return response.Value.Content[0].Text;
        }

        public async Task<string> AnalyzeProjectRiskAsync(
            string projectName,
            string complexity,
            int priority,
            int minimumExperience,
            List<string> requiredSkills)
        {
            var prompt = $"""
            You are a project risk analysis expert.

            Analyze this project and provide:
            1. Overall risk summary
            2. Top risk factors
            3. Mitigation recommendations

            Project Name: {projectName}
            Complexity: {complexity}
            Priority: {priority}
            Minimum Experience: {minimumExperience}
            Required Skills: {string.Join(", ", requiredSkills)}
            """;

            var response = await _chatClient.CompleteChatAsync(prompt);

            return response.Value.Content[0].Text;
        }
    }
}