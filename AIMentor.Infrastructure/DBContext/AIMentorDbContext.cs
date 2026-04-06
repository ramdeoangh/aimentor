

using AIMentor.Domain;
using Microsoft.EntityFrameworkCore;

namespace AIMentor.Infrastructure.DBContext
{
    public class AIMentorDbContext : DbContext
    {
        public AIMentorDbContext(DbContextOptions<AIMentorDbContext> options)
            : base(options) { }

        public DbSet<Document> Documents => Set<Document>();
        public DbSet<DocumentChunk> Chunks => Set<DocumentChunk>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("vector");

            modelBuilder.Entity<DocumentChunk>()
                .Property(x => x.Embedding)
                .HasColumnType("vector(1536)");
        }
    }
}
