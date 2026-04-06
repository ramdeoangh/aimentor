namespace AIMentor.Application;

public interface IRagService
{
    IAsyncEnumerable<string> StreamRagResponseAsync(
        string userQuestion,
        int topK,
        CancellationToken cancellationToken = default);
}
