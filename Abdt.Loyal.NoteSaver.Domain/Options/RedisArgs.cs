using System.ComponentModel.DataAnnotations;

namespace Abdt.Loyal.NoteSaver.Domain.Options
{
    public class RedisArgs
    {
        [Required]
        public uint DefaultCacheDurationInMinutes { get; set; }
    }
}
