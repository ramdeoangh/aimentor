using AIMentor.Application;
using AIMentor.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AIMentor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ILLMService _llmService;

        public ChatController(ILLMService llmService)
        {
            _llmService = llmService;
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request)
        {
            var response = await _llmService.GetChatResponseAsync(request.Message);
            return Ok(new { reply = response });
        }

        [HttpPost("stream")]
        public async Task Stream([FromBody] ChatRequest request)
        {
            Response.Headers.Append("Content-Type", "text/event-stream");

            await foreach (var chunk in _llmService.StreamChatResponseAsync(request.Message))
            {
                await Response.WriteAsync($"data: {chunk}\n\n");
                await Response.Body.FlushAsync();
            }
        }

        [HttpPost("store")]
        public async Task<IActionResult> StoreTest([FromBody] ChatRequest request,
            [FromServices] IEmbeddingService embeddingService,
            [FromServices] IVectorRepository vectorRepository)
        {
            var embedding = await embeddingService.GenerateEmbeddingAsync(request.Message);

            await vectorRepository.AddChunkAsync(request.Message, embedding);

            return Ok("Stored successfully");

        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchTest(
            [FromBody] ChatRequest request,
            [FromServices] IEmbeddingService embeddingService,
            [FromServices] IVectorRepository vectorRepository)
        {
            var embedding = await embeddingService.GenerateEmbeddingAsync(request.Message);

            var results = await vectorRepository.SearchSimilarAsync(embedding, 3);

            return Ok(results.Select(r => r.Content));
        }

        public class ChatRequest
        {
            public string Message { get; set; }
        }
    }
}
