using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Neksara.Models
{
    public class Topic
    {
        [Key]
        public int TopicId { get; set; }

        [Required]
        public string TopicName { get; set; } = null!;

        public string? Body { get; set; }

        public string? VideoUrl { get; set; }

        public string? PictTopic { get; set; }

        public int ViewCount { get; set; } = 0;

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // FK
        [ForeignKey("Category")]
        public int IdCategory { get; set; }

        public Category? Category { get; set; }

        // Navigation
        public ICollection<TopicView>? TopicViews { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
    }
}