using System.Collections.Generic;
using Neksara.Models; // Add this if Topic is defined in Models namespace

namespace Neksara.ViewModels
{
    public class SearchViewModel
    {
        public string? Keyword { get; set; }

        public List<Topic> Topics { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
    }
}