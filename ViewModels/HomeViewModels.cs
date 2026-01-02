using Neksara.Models;
using System.Collections.Generic;

namespace Neksara.ViewModels
{
    public class HomeViewModel
    {
        public List<Topic> PopularTopics { get; set; } = new List<Topic>();
        public List<Category> Categories { get; set; } = new List<Category>();
    }
}