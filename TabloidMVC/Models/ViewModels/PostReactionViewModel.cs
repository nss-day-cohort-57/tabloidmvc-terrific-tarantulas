using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class PostReactionViewModel
    {
        public PostReaction postReaction { get; set; }

        public List<Reaction> allReactions { get; set; }
    }
}
