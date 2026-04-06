using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using AIMentor.Application;
using System.Threading.Tasks;

namespace AIMentor.Infrastructure;

public class OpenAiService : ILLMService
{
    private readonly string _apiKey;
    private readonly string _model;

    public OpenAiService(IConfiguration configuration)
    {
        _apiKey = OpenAiConfiguration.GetApiKey(configuration);
        _model = OpenAiConfiguration.GetChatModel(configuration);
    }

    public async Task<string> GetChatResponseAsync(string userMessage)
    {
        var client = new ChatClient(_model, _apiKey);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage("You are an AI Engineering Mentor helping a software engineer learn AI deeply."),
            new UserChatMessage(userMessage)
        };

        var response = await client.CompleteChatAsync(messages);

        return response.Value.Content[0].Text;
    }

    public async IAsyncEnumerable<string> StreamChatResponseAsync(string userMessage)
    {
        var client = new ChatClient(_model, _apiKey);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage("You are an AI Engineering Mentor helping a software engineer learn AI deeply."),
            new UserChatMessage(userMessage)
        };

        await foreach (var update in client.CompleteChatStreamingAsync(messages))
        {
            foreach (var content in update.ContentUpdate)
            {
                yield return content.Text;
            }
        }
    }
}
