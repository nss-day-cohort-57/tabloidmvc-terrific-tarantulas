using System.ComponentModel.DataAnnotations;

namespace TabloidMVC.Models
{
    public class Reaction
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageLocation { get; set; }
    }
}
