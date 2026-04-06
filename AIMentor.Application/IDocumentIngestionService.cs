namespace AIMentor.Application;

public interface IDocumentIngestionService
{
    Task IngestAsync(string title, string content, CancellationToken cancellationToken = default);
}
