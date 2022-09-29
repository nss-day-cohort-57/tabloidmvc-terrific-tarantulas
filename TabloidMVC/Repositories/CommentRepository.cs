using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class CommentRepository: BaseRepository, ICommentRepository
    {
        public CommentRepository(IConfiguration config) : base(config) { }

        public Comment GetCommentById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT com.Id, com.Subject, com.Content, com.CreateDateTime AS CommentCreateDate, com.PostId,
                              com.UserProfileId, p.Title AS PostTitle, p.Content AS PostContent, p.ImageLocation AS HeaderImage,
                              p.CreateDateTime AS PostCreateDate, p.PublishDateTime, p.CategoryId, up.FirstName, up.LastName,
                              up.DisplayName, up.Email, up.CreateDateTime AS UserCreateDate, up.ImageLocation AS AvatarImage,
                              up.UserTypeId, ut.Name AS UserTypeName
                         FROM Comment com
                              LEFT JOIN Post p ON p.Id = com.PostId
                              LEFT JOIN UserProfile up ON up.Id = com.UserProfileId
                              LEFT JOIN UserType ut ON ut.Id = up.UserTypeId
                        WHERE com.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    Comment comment = null;

                    if (reader.Read())
                    {
                        comment = ReadComment(reader);
                    }

                    reader.Close();

                    return comment;
                }
            }
        }
        public void AddComment(Comment comment)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Comment (
                            Subject, Content, CreateDateTime, PostId, UserProfileId )
                        OUTPUT INSERTED.ID
                        VALUES (
                            @Subject, @Content, @CreateDateTime, @PostId, @UserProfileId  )";
                    cmd.Parameters.AddWithValue("@Subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@Content", comment.Content);
                    cmd.Parameters.AddWithValue("@CreateDateTime", comment.CreateDateTime);
                    cmd.Parameters.AddWithValue("@PostId", comment.PostId);
                    cmd.Parameters.AddWithValue("@UserProfileId", comment.UserProfileId);


                    comment.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
        public void DeleteComment(int commentId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM comment
                                        WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", commentId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateComment(Comment comment)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Comment
                            SET 
                                Subject = @Subject,
                                Content = @Content,
                                CreateDateTime = @CreateDateTime,
                                PostId = @PostId,
                                UserProfileId = @UserProfileId,
                            WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@Subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@Content", comment.Content);
                    cmd.Parameters.AddWithValue("@CreateDateTime", comment.CreateDateTime);
                    cmd.Parameters.AddWithValue("@PostId", comment.PostId);
                    cmd.Parameters.AddWithValue("@UserProfileId", comment.UserProfileId);
                    cmd.Parameters.AddWithValue("@id", comment.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private Comment ReadComment(SqlDataReader reader)
        {
            return new Comment()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Subject = reader.GetString(reader.GetOrdinal("Subject")),
                Content = reader.GetString(reader.GetOrdinal("Content")),
                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CommentCreateDate")),
                PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                Post = new Post()
                {
                    Title = reader.GetString(reader.GetOrdinal("PostTitle")),
                    Content = reader.GetString(reader.GetOrdinal("PostContent")),
                    ImageLocation = DbUtils.GetNullableString(reader, "HeaderImage"),
                    CreateDateTime = reader.GetDateTime(reader.GetOrdinal("PostCreateDate")),
                    PublishDateTime = DbUtils.GetNullableDateTime(reader, "PublishDateTime"),
                    CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                },
                UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                UserProfile = new UserProfile()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                    DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    CreateDateTime = reader.GetDateTime(reader.GetOrdinal("UserCreateDate")),
                    ImageLocation = DbUtils.GetNullableString(reader, "AvatarImage"),
                    UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                    UserType = new UserType()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                        Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                    }
                }
            };
        }
    }
}
