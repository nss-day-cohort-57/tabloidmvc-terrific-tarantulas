using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ITagRepository
    {
        List<Tag> GetAll();
        Tag GetTagById(int id);
        void Add(Tag tag);
        void Delete(int tagId);
        void UpdateTag(Tag tag);
    }
}