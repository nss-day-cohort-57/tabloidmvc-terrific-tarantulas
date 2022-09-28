using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Xml.Linq;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class ReactionRepository : BaseRepository, IReactionRepository
    {
        public ReactionRepository(IConfiguration config) : base(config) { }

        public List<Reaction> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, ImageLocation FROM Reaction";
                    var reader = cmd.ExecuteReader();

                    var reactions = new List<Reaction>();

                    while (reader.Read())
                    {
                        reactions.Add(new Reaction()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            ImageLocation = reader.GetString(reader.GetOrdinal("ImageLocation"))
                        });
                    }

                    reader.Close();

                    return reactions;
                }
            }
        }
    }
}
