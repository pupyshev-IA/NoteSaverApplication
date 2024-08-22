using Abdt.Loyal.NoteSaver.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abdt.Loyal.NoteSaver.Repository
{
    internal class NoteEntityConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.HasQueryFilter(q => !EF.Property<bool>(q, "IsDeleted"));
        }
    }
}
