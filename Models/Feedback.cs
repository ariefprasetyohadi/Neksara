using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Neksara.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }

        [Required]
        public string TargetType { get; set; } = null!; // Topic / Category

        [Required]
        public int TargetId { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        public int? Rating { get; set; }

        public bool IsApproved { get; set; } = false;
        public bool IsVisible { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // FK User
        [ForeignKey("User")]
        public int IdUser { get; set; }
        public User? User { get; set; }
    }
}