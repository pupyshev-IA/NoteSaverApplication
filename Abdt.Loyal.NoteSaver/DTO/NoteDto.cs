using Abdt.Loyal.NoteSaver.Domain;
using System.ComponentModel.DataAnnotations;

namespace Abdt.Loyal.NoteSaver.DTO
{
    public class NoteDto
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(50, ErrorMessage = "Title length more than 50 symbols is not allowed")]
        public required string Title { get; set; }
        
        [MaxLength(2000, ErrorMessage = "Content length more than 2000 symbols is not allowed")]
        public string? Content { get; set; }

        [Required(ErrorMessage = "Status must be specified")]
        public NoteStatus Status { get; set; }
    }
}
