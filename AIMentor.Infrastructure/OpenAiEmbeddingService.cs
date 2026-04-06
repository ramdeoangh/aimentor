using AIMentor.Application;
using Microsoft.Extensions.Configuration;
using OpenAI.Embeddings;

namespace AIMentor.Infrastructure;

public class OpenAiEmbeddingService : IEmbeddingService
{
    private readonly string _apiKey;

    private readonly string _embeddingModel;

    public OpenAiEmbeddingService(IConfiguration configuration)
    {
        _apiKey = OpenAiConfiguration.GetApiKey(configuration);
        _embeddingModel = OpenAiConfiguration.GetEmbeddingModel(configuration);
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text)
    {
        var client = new EmbeddingClient(_embeddingModel, _apiKey);

        var response = await client.GenerateEmbeddingAsync(text);

        // v2.x structure
        return response.Value.ToFloats().ToArray();
    }
}