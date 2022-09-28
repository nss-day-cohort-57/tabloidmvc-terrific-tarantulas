using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IReactionRepository
    {
        List<Reaction> GetAll();
    }
}
