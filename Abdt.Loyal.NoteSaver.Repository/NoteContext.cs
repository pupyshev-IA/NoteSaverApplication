using Abdt.Loyal.NoteSaver.Domain;
using Microsoft.EntityFrameworkCore;

namespace Abdt.Loyal.NoteSaver.Repository
{
    public class NoteContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }

        public NoteContext(DbContextOptions<NoteContext> options) : base(options)
        {

        }
    }
}
