using Abdt.Loyal.NoteSaver.Domain;

namespace Abdt.Loyal.NoteSaver.DTO
{
    public class NoteDtoOut
    {
        public long Id { get; set; }

        public required string Title { get; set; }

        public string? Content { get; set; }

        public NoteStatus Status { get; set; }
    }
}
