using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
        Comment GetCommentById(int id);
        void AddComment(Comment comment);
        void DeleteComment(int commentId);
        void UpdateComment(Comment comment);
    }
}
