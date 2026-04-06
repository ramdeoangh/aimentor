namespace AIMentor.Application;

public interface ILLMService
{
    Task<string> GetChatResponseAsync(string userMessage);

    IAsyncEnumerable<string> StreamChatResponseAsync(string userMessage);

    IAsyncEnumerable<string> StreamChatResponseAsync(string systemPrompt, string userMessage);
}
