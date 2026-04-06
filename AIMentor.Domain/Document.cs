using Pgvector;

namespace AIMentor.Domain;

public class Document
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public List<DocumentChunk> Chunks { get; set; } = new();
}
public class DocumentChunk
{
    public Guid Id { get; set; }
    public Guid DocumentId { get; set; }
    public string Content { get; set; } = default!;
    public Vector Embedding { get; set; } = default!;

    public Document Document { get; set; } = default!;
}
