using System.ComponentModel.DataAnnotations;

namespace OnTheBlog.Models
{
    public class Comment
    {
        private DateTime _created;
        private DateTime? _updated;

        //primary key
        public int Id { get; set; }
        public DateTime Created 
        {
            get => _created; 
            set => _created = value.ToUniversalTime();
        }
        public DateTime? Updated 
        {
            get => _updated; 
            set => _updated = value.HasValue ? value.Value.ToUniversalTime() : null;
        }

        [Display(Name = "Reason for Update")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 2)]
        public string? UpdateReason { get; set; }

        [Required]
        [Display(Name = "Comment")]
        [StringLength(5000, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 2)]
        public string? Body { get; set; }

        [Required]
        public string? AuthorId { get; set; }

        public virtual BlogUser? Author { get; set; }

        public int BlogPostId { get; set; }
        public virtual BlogPost? BlogPost { get; set; }

    }
}
