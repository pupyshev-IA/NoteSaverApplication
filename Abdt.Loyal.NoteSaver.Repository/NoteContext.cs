using Abdt.Loyal.NoteSaver.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Abdt.Loyal.NoteSaver.Repository
{
    public class NoteContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }

        public NoteContext(DbContextOptions<NoteContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Note>(e =>
            {
                e.HasKey(e => e.Id);
                e.Property(p => p.Id).ValueGeneratedOnAdd();
                e.Property(p => p.Title).HasMaxLength(50);
                e.Property(p => p.Content).HasMaxLength(2000);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();

            base.OnConfiguring(optionsBuilder);
        }
    }
}
