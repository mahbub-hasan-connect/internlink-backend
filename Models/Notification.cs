using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternLink.Api.Models
{
    // Module 7: Notification System
    // যখনই Application-এর Status বদলায় (Module 6), এখানে একটা নতুন Notification তৈরি হবে
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        // কার জন্য এই notification (সাধারণত Student, যার application status বদলেছে)
        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}