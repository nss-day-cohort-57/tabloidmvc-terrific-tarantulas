using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TabloidMVC.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        [DisplayName("Created")]
        [DataType(DataType.Date)]
        public DateTime CreateDateTime { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        [DisplayName("Author")]
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}

