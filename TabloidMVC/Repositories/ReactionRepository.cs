using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Xml.Linq;
using TabloidMVC.Models;
using TabloidMVC.Utils;

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
                        reactions.Add(ReadReaction(reader));
                    }

                    reader.Close();

                    return reactions;
                }
            }
        }

        public List<PostReaction> GetPostReactions(int postId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT * FROM PostReaction
                       WHERE PostId = @postId";

                    cmd.Parameters.AddWithValue("@postId", postId);

                    var reader = cmd.ExecuteReader();

                    var prs = new List<PostReaction>();

                    while (reader.Read())
                    {
                        prs.Add(ReadPostReaction(reader));
                    }

                    reader.Close();

                    return prs;
                }
            }
        }

        public List<Reaction> GetReactionsByPost(int postId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            SELECT r.* FROM PostReaction pr
                            JOIN Reaction r ON r.Id = pr.ReactionId
                            WHERE pr.PostId = @postId";

                    cmd.Parameters.AddWithValue("@postId", postId);

                    var reader = cmd.ExecuteReader();

                    var reactions = new List<Reaction>();

                    while (reader.Read())
                    {
                        reactions.Add(ReadReaction(reader));
                    }

                    reader.Close();

                    return reactions;
                }
            }
        }

        public void Add(Reaction reaction)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Reaction (ImageLocation,[Name])
                        OUTPUT INSERTED.ID
                        VALUES (@rImage,@rName)";
                    cmd.Parameters.AddWithValue("@rImage", reaction.ImageLocation);
                    cmd.Parameters.AddWithValue("@rName", reaction.Name);
                    reaction.Id = (int)cmd.ExecuteScalar();
                }
            }
        }


        public PostReaction ReadPostReaction(SqlDataReader reader)
        {
            return new PostReaction()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                ReactionId = reader.GetInt32(reader.GetOrdinal("ReactionId")),
                UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId"))
            };
        }

        public Reaction ReadReaction(SqlDataReader reader)
        {
            return new Reaction()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                ImageLocation = reader.GetString(reader.GetOrdinal("ImageLocation"))
            };
        }
    }
}
