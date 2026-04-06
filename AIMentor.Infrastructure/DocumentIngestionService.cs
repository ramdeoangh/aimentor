using AIMentor.Application;
using AIMentor.Domain;
using Microsoft.Extensions.Configuration;

namespace AIMentor.Infrastructure;

public class DocumentIngestionService : IDocumentIngestionService
{
    private readonly IEmbeddingService _embeddingService;
    private readonly IVectorRepository _vectorRepository;
    private readonly int _maxChunkChars;
    private readonly int _overlapChars;

    public DocumentIngestionService(
        IEmbeddingService embeddingService,
        IVectorRepository vectorRepository,
        IConfiguration configuration)
    {
        _embeddingService = embeddingService;
        _vectorRepository = vectorRepository;
        _maxChunkChars = configuration.GetValue("Chunking:MaxChunkChars", 2800);
        _overlapChars = configuration.GetValue("Chunking:OverlapChars", 400);
    }

    public async Task IngestAsync(string title, string content, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentNullException.ThrowIfNull(content);

        if (content.Length == 0)
            return;

        var segments = SplitIntoOverlappingChunks(content, _maxChunkChars, _overlapChars).ToList();
        if (segments.Count == 0)
            return;

        var document = new Document
        {
            Id = Guid.NewGuid(),
            Title = title,
            Content = content
        };

        var chunkData = new List<(string Content, float[] Embedding)>();
        foreach (var segment in segments)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var embedding = await _embeddingService.GenerateEmbeddingAsync(segment);
            chunkData.Add((segment, embedding));
        }

        await _vectorRepository.AddDocumentWithChunksAsync(document, chunkData);
    }

    /// <summary>
    /// Character-based windows with overlap; not true token chunking (see roadmap).
    /// </summary>
    internal static IEnumerable<string> SplitIntoOverlappingChunks(string text, int maxChars, int overlap)
    {
        if (maxChars <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxChars));

        overlap = Math.Clamp(overlap, 0, Math.Max(0, maxChars - 1));
        var step = Math.Max(1, maxChars - overlap);

        var start = 0;
        while (start < text.Length)
        {
            var len = Math.Min(maxChars, text.Length - start);
            yield return text.Substring(start, len);
            if (start + len >= text.Length)
                break;
            start += step;
        }
    }
}
