using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostRepository
    {
        void Add(Post post);
        void Delete(int postId);
        void AddPostReaction(PostReaction postReaction);
        List<Post> GetAllPublishedPosts();
        Post GetPublishedPostById(int id);
        Post GetUserPostById(int id, int userProfileId);
        void UpdatePost(Post post);
        Post GetPostById(int id);
        List<Post> GetCurrentUsersPosts(int userProfileId);
    }
}