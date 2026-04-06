namespace AIMentor.Application;

public interface IEmbeddingService
{
    Task<float[]> GenerateEmbeddingAsync(string text);
}
