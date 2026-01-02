using System.Collections.Generic;
using Neksara.Models;

namespace Neksara.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalCategories { get; set; }
        public int TotalTopics { get; set; }
        public int TotalFeedbacks { get; set; }
        public int PendingFeedbacks { get; set; }

        public List<Topic> PopularTopics { get; set; } = new List<Topic>();
    }
}