namespace Abdt.Loyal.NoteSaver.Domain
{
    public class Note
    {
        public long Id { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public NoteStatus Status { get; set; }
    }

    public enum NoteStatus
    {
        InProgress,
        Done,
        Pending,
        Cancelled
    }
}
