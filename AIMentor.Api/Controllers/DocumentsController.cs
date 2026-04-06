using AIMentor.Application;
using Microsoft.AspNetCore.Mvc;

namespace AIMentor.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentIngestionService _ingestionService;

    public DocumentsController(IDocumentIngestionService ingestionService)
    {
        _ingestionService = ingestionService;
    }

    [HttpPost("ingest")]
    public async Task<IActionResult> Ingest([FromBody] IngestRequest request, CancellationToken cancellationToken)
    {
        await _ingestionService.IngestAsync(request.Title, request.Content, cancellationToken);
        return Ok(new { status = "ingested" });
    }

    public class IngestRequest
    {
        public string Title { get; set; } = default!;

        public string Content { get; set; } = default!;
    }
}
