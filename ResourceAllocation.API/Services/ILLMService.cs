namespace ResourceAllocation.API.Services
{
    public interface ILLMService
    {
        Task<string> AskAsync(string prompt);
    }
}