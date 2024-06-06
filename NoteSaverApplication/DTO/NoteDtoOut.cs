using System.ComponentModel.DataAnnotations;

namespace Abdt.Loyal.NoteSaver.DTO
{
    public class NoteDtoOut
    {
        public long Id { get; set; }

        public required string Title { get; set; }

        public string? Content { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }
    }
}
