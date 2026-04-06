using AIMentor.Application;
using AIMentor.Infrastructure;
using AIMentor.Infrastructure.DBContext;
using AIMentor.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AIMentor API",
        Version = "v1",
        Description = "RAG, embeddings, document ingestion, and chat. Uses OpenAI and PostgreSQL with pgvector."
    });
});
builder.Services.AddScoped<ILLMService, OpenAiService>();
builder.Services.AddScoped<IEmbeddingService, OpenAiEmbeddingService>();
builder.Services.AddScoped<IVectorRepository, VectorRepository>();
builder.Services.AddScoped<IRagService, RagService>();
builder.Services.AddScoped<IDocumentIngestionService, DocumentIngestionService>();

builder.Services.AddDbContext<AIMentorDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        o => o.UseVector()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "AIMentor API v1");
    });
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();