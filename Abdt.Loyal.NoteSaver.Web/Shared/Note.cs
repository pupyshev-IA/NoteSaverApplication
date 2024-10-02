namespace Abdt.Loyal.NoteSaver.Web.Shared
{
    public class Note
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
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
