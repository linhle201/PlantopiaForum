using PlantopiaForum.Data;

namespace PlantopiaForum.Models
{
    public class Comment
    {
        public int CommentId { get; set; }         // Primary Key
        public string Content { get; set; } = string.Empty;        // Content 
        public DateTime CreateDate { get; set; }   // Date the comment was created

        // Foreign Key
        public int DiscussionId { get; set; }      // This links to  discussion

        // Navigation property for the related discussion
        public Discussion? Discussion { get; set; } //nullable!!

        public string ApplicationUserId { get; set; } = string.Empty;
        // Navigation property
        public ApplicationUser? ApplicationUser { get; set; } // nullable!!!
    }
}
