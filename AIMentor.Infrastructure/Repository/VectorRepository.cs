using AIMentor.Domain;
using AIMentor.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace AIMentor.Infrastructure.Repository
{
    public class VectorRepository : IVectorRepository
    {
        private readonly AIMentorDbContext _context; 

        public VectorRepository(AIMentorDbContext context)
        {
            _context = context;
        }

        public async Task AddChunkAsync(string content, float[] embedding)
        {
            // Create a Document
            var document = new Document
            {
                Id = Guid.NewGuid(),
                Title = "Test Document",
                Content = content
            };

            var chunk = new DocumentChunk
            {
                Id = Guid.NewGuid(),
                DocumentId = document.Id,
                Content = content,
                Embedding = new Vector(embedding),
                Document = document
            };

            _context.Documents.Add(document);
            _context.Chunks.Add(chunk);

            await _context.SaveChangesAsync();
        }

        public async Task<List<DocumentChunk>> SearchSimilarAsync(float[] embedding, int topK)
        {
            var vector = new Vector(embedding);

            return await _context.Chunks
                .OrderBy(c => c.Embedding.CosineDistance(vector))
                .Take(topK)
                .ToListAsync();
        }
    }
}
