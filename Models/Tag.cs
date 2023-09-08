using System.ComponentModel.DataAnnotations;

namespace OnTheBlog.Models
{
    public class Tag
    {
        //primary key
        public int Id { get; set; }
        [Required]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 2)]

        public string? Name { get; set; }

        public ICollection<BlogPost> BlogPosts { get; set; } = new HashSet<BlogPost>();
    }

}
