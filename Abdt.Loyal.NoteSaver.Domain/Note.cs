namespace Abdt.Loyal.NoteSaver.Domain
{
    public class Note
    {
        public long Id { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public long UserId { get; set; }
    }
}
