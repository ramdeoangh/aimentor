using AIMentor.Application;
using AIMentor.Infrastructure;
using AIMentor.Infrastructure.DBContext;
using AIMentor.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<ILLMService, OpenAiService>();
builder.Services.AddScoped<IEmbeddingService, OpenAiEmbeddingService>();
builder.Services.AddScoped<IVectorRepository, VectorRepository>();

builder.Services.AddDbContext<AIMentorDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        o => o.UseVector()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();