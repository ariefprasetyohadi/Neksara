using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Neksara.Models
{
    public class TopicView
    {
        [Key]
        public int TopicViewId { get; set; }

        public DateTime ViewAt { get; set; } = DateTime.UtcNow;

        // FK Topic
        [ForeignKey("Topic")]
        public int IdTopic { get; set; }
        public Topic? Topic { get; set; }

        // FK User (nullable kalau anonymous)
        [ForeignKey("User")]
        public int? IdUser { get; set; }
        public User? User { get; set; }
    }
}