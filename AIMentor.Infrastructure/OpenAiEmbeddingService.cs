using AIMentor.Application;
using Microsoft.Extensions.Configuration;
using OpenAI.Embeddings;

namespace AIMentor.Infrastructure;

public class OpenAiEmbeddingService : IEmbeddingService
{
    private readonly string _apiKey;

    public OpenAiEmbeddingService(IConfiguration configuration)
    {
        _apiKey = configuration["OpenAI:ApiKey"]!;
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text)
    {
        var client = new EmbeddingClient("text-embedding-3-small", _apiKey);

        var response = await client.GenerateEmbeddingAsync(text);

        // v2.x structure
        return response.Value.ToFloats().ToArray();
    }
}