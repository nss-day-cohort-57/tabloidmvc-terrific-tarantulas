using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class PostsViewModel
    {
        public int CategoryId { get; set; }
        public List<Post> Posts { get; set; }
        public List<Category> Categories { get; set; }
    }
}
