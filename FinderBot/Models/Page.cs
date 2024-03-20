using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinderBot.Models
{
    public class Page
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string URL { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Content { get; set; }
        public List<string> Links { get; set; }
        public List<string> MediaLinks { get; set; }
        [Required]
        public DateTime IndexTime { get; set; }
    }
}
