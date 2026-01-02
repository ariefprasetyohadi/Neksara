using System.ComponentModel.DataAnnotations;

namespace Neksara.Models
{
    public class Category
    {
        [Key]
        public int CategoriesId { get; set; }

        [Required]
        public string CategoryName { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateAt { get; set; }

        // Navigation
        public ICollection<Topic> Topics { get; set; } = new List<Topic>();
    }
}