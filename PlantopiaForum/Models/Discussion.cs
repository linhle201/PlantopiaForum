using System.ComponentModel.DataAnnotations;

namespace PlantopiaForum.Models
{
    public class Discussion
    {
        public int DiscussionId { get; set; }         // Primary Key
        public string Title { get; set; } = string.Empty;             // Title 
        public string Content { get; set; } = string.Empty;          // Content 

        public string ImageFilename { get; set; } = string.Empty;

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation property 
        public List<Comment> ? Comments { get; set; }  // nullable!!!

    }
}
