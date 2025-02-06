using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantopiaForum.Models
{
    public class Discussion
    {
        public int DiscussionId { get; set; }         // Primary Key
        public string Title { get; set; } = string.Empty;             // Title 
        public string Content { get; set; } = string.Empty;          // Content 

        public string ImageFilename { get; set; } = string.Empty;

        // Property for file upload, not mapped in EF
        [NotMapped]
        [Display(Name = "Photograph")]
        public IFormFile? ImageFile { get; set; } // nullable! 

        [Display(Name = "Posted on")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation property 
        public List<Comment> ? Comments { get; set; }  // nullable!!!

    }
}
