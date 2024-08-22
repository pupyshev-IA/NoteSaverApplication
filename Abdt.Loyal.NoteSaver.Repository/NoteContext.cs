using Abdt.Loyal.NoteSaver.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Abdt.Loyal.NoteSaver.Repository
{
    public class NoteContext : DbContext
    {
        private const string IsDeletedPropertyName = "IsDeleted";
        private const string DateOfCreation = "CreatedAt";
        private const string DateOfUpdating = "UpdatedAt";
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
                e.Property<DateTimeOffset>(DateOfCreation);
                e.Property<DateTimeOffset>(DateOfUpdating);
                e.Property<bool>(IsDeletedPropertyName);
            });

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();

            base.OnConfiguring(optionsBuilder);
        }

        public override int SaveChanges()
        {
            ProccessEntityChanges();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ProccessEntityChanges();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ProccessEntityChanges()
        {
            ChangeTracker.DetectChanges();
            foreach (var entry in ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted))
            {
                if (entry.Properties.Any(p => p.Metadata.Name == IsDeletedPropertyName))
                {
                    entry.Property(IsDeletedPropertyName).CurrentValue = true;
                    entry.State = EntityState.Modified;
                }
            }

            var utcNow = DateTimeOffset.UtcNow;
            foreach (var entry in ChangeTracker.Entries().Where(x => x.State is EntityState.Added or EntityState.Modified))
            {
                ProccessAuditProperties(entry, utcNow);
            }
        }

        private void ProccessAuditProperties(EntityEntry entry, DateTimeOffset dt)
        {
            if (entry.Properties.Any(p => p.Metadata.Name == DateOfUpdating))
            {
                entry.Property(DateOfUpdating).CurrentValue = dt;
            }

            if (entry.State == EntityState.Added)
            {
                if (entry.Properties.Any(p => p.Metadata.Name == DateOfCreation))
                {
                    entry.Property(DateOfCreation).CurrentValue = dt;
                }
                else
                {
                    if (entry.Properties.Any(p => p.Metadata.Name == DateOfCreation))
                    {
                        entry.Property(DateOfCreation).IsModified = false;
                    }
                }
            }
        }
    }
}
