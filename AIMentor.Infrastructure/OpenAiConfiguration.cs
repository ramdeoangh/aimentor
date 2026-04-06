using Microsoft.Extensions.Configuration;

namespace AIMentor.Infrastructure;

internal static class OpenAiConfiguration
{
    public static string GetApiKey(IConfiguration configuration)
    {
        var key = configuration["OpenAI:ApiKey"]
            ?? configuration["OpenAI:APIKey"];

        if (string.IsNullOrWhiteSpace(key))
            key = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidOperationException(
                "OpenAI API key is not configured. Set user secret OpenAI:ApiKey, environment variable " +
                "OPENAI_API_KEY or OpenAI__ApiKey, or OpenAI:ApiKey in configuration.");
        }

        return key;
    }

    public static string GetChatModel(IConfiguration configuration, string defaultModel = "gpt-4o-mini")
    {
        var model = configuration["OpenAI:Model"];
        return string.IsNullOrWhiteSpace(model) ? defaultModel : model;
    }

    public static string GetEmbeddingModel(IConfiguration configuration, string defaultModel = "text-embedding-3-small")
    {
        var model = configuration["OpenAI:EmbeddingModel"];
        return string.IsNullOrWhiteSpace(model) ? defaultModel : model;
    }
}
