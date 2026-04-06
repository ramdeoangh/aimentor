using System.Runtime.CompilerServices;
using System.Text;
using AIMentor.Application;
using AIMentor.Domain;
using Microsoft.Extensions.Configuration;

namespace AIMentor.Infrastructure;

public class RagService : IRagService
{
    public const string NotEnoughContextReply = "I don't have enough information from the stored knowledge.";

    private readonly IEmbeddingService _embeddingService;
    private readonly IVectorRepository _vectorRepository;
    private readonly ILLMService _llmService;
    private readonly int _defaultTopK;

    public RagService(
        IEmbeddingService embeddingService,
        IVectorRepository vectorRepository,
        ILLMService llmService,
        IConfiguration configuration)
    {
        _embeddingService = embeddingService;
        _vectorRepository = vectorRepository;
        _llmService = llmService;
        _defaultTopK = configuration.GetValue("Rag:TopK", 5);
    }

    public async IAsyncEnumerable<string> StreamRagResponseAsync(
        string userQuestion,
        int topK,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (topK <= 0)
            topK = _defaultTopK;

        var queryEmbedding = await _embeddingService.GenerateEmbeddingAsync(userQuestion);
        cancellationToken.ThrowIfCancellationRequested();

        var chunks = await _vectorRepository.SearchSimilarAsync(queryEmbedding, topK);

        if (chunks.Count == 0 || chunks.TrueForAll(c => string.IsNullOrWhiteSpace(c.Content)))
        {
            yield return NotEnoughContextReply;
            yield break;
        }

        var systemPrompt = BuildRagSystemPrompt(chunks);

        await foreach (var token in _llmService.StreamChatResponseAsync(systemPrompt, userQuestion))
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return token;
        }
    }

    private static string BuildRagSystemPrompt(List<DocumentChunk> chunks)
    {
        var sb = new StringBuilder();
        sb.AppendLine("You are an AI Engineering Mentor helping a software engineer learn AI deeply.");
        sb.AppendLine("Answer using ONLY the retrieved context below. Do not invent facts beyond it.");
        sb.Append("If the context does not contain enough information to answer the user's question, ");
        sb.Append("respond with exactly this sentence and nothing else: ");
        sb.Append('"');
        sb.Append(NotEnoughContextReply);
        sb.AppendLine("\"");
        sb.AppendLine();
        sb.AppendLine("Retrieved context:");
        for (var i = 0; i < chunks.Count; i++)
        {
            var text = chunks[i].Content.Trim();
            if (text.Length == 0)
                continue;
            sb.Append('[');
            sb.Append(i + 1);
            sb.Append("] ");
            sb.AppendLine(text);
        }

        return sb.ToString();
    }
}
