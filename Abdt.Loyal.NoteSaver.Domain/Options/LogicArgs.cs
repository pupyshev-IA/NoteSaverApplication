using System.ComponentModel.DataAnnotations;

namespace Abdt.Loyal.NoteSaver.Domain.Options
{
    public class LogicArgs
    {
        [Required]
        public bool UseSoftDelete { get; set; }
    }
}
