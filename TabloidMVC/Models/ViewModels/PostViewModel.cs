using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class PostViewModel
    {
        public int currentuserId { get; set; }
        public Post post { get; set; }
        public List<Reaction> reactions { get; set; }


        //To be added later on.
        //public List<Comment> comments { get; set; }
    }

}
