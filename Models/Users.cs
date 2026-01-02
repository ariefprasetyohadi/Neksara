using System.ComponentModel.DataAnnotations;

namespace Neksara.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? PhotoUrl { get; set; }

        // Admin / User
        [Required]
        public string Role { get; set; } = "User";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<TopicView>? TopicViews { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
    }
}