using System.ComponentModel.DataAnnotations.Schema;

namespace Abdt.Loyal.NoteSaver.Domain
{
    [Table("notes")]
    public class Note
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("title")]
        public string? Title { get; set; }

        [Column("content")]
        public string? Content { get; set; }

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
