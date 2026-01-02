using System.ComponentModel.DataAnnotations;

namespace Neksara.Models
{
    public class SearchLog
    {
        [Key]
        public int SearchLogId { get; set; }

        [Required]
        public string Keyword { get; set; } = null!;

        public int ResultCount { get; set; }

        public DateTime SearchedAt { get; set; } = DateTime.UtcNow;
    }
}