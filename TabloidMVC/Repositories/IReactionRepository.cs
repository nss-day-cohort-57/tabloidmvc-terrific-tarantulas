using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IReactionRepository
    {
        List<Reaction> GetAll();
        List<PostReaction> GetPostReactions(int postId);
        List<Reaction> GetReactionsByPost(int postId);

        void Add(Reaction reaction);
    }
}
