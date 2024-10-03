namespace Abdt.Loyal.NoteSaver.Domain
{
    public class NoteOut
    {
        public long Id { get; set; }

        public required string Title { get; set; }

        public string? Content { get; set; }

        public NoteStatus Status { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }
    }
}
