using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace ResourceAllocation.API.Services
{
    public class LLMService : ILLMService
    {
        private readonly ChatClient _chatClient;

        public LLMService(IConfiguration configuration)
        {
            var apiKey = configuration["OpenAI:LLM:ApiKey"];
            var baseUrl = configuration["OpenAI:LLM:BaseUrl"];
            var model = configuration["OpenAI:LLM:Model"];

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new Exception("LLM API key is missing.");

            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new Exception("LLM BaseUrl is missing.");

            if (string.IsNullOrWhiteSpace(model))
                throw new Exception("LLM Model is missing.");

            var options = new OpenAIClientOptions
            {
                Endpoint = new Uri(baseUrl)
            };

            var client = new OpenAIClient(
                new ApiKeyCredential(apiKey),
                options);

            _chatClient = client.GetChatClient(model);
        }

        public virtual async Task<string> AskAsync(string prompt)
        {
            var response = await _chatClient.CompleteChatAsync(prompt);

            return response.Value.Content[0].Text;
        }
    }
}