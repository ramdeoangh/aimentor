using AIMentor.Domain;

namespace AIMentor.Infrastructure.Repository
{
    public interface IVectorRepository
    {
        Task AddChunkAsync(string content, float[] embedding);
        Task<List<DocumentChunk>> SearchSimilarAsync(float[] embedding, int topK);
    }
}
