using AIMentor.Domain;

namespace AIMentor.Application;

public interface IVectorRepository
{
    Task AddChunkAsync(string content, float[] embedding);

    Task AddDocumentWithChunksAsync(Document document, IReadOnlyList<(string Content, float[] Embedding)> chunks);

    Task<List<DocumentChunk>> SearchSimilarAsync(float[] embedding, int topK);
}
