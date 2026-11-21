using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternLink.Api.Models
{
    // Module 6: Application & Status Tracking
    // এটাই InternLink-এর মূল কাজ — Student কোন JobPosting-এ apply করেছে তার রেকর্ড
    // ApplicationStatus enum দিয়ে ৫-ধাপের pipeline track করা হয় (তোমার UR3 অনুযায়ী)
    public enum ApplicationStatus
    {
        Applied,
        Shortlisted,
        Interview,
        Offered,
        Hired,
        Rejected
    }

    public class Application
    {
        [Key]
        public int Id { get; set; }

        // Foreign Key ১ — কোন Student apply করেছে
        [Required]
        public int StudentUserId { get; set; }

        [ForeignKey("StudentUserId")]
        public User? Student { get; set; }

        // Foreign Key ২ — কোন JobPosting-এ apply করা হয়েছে
        [Required]
        public int JobPostingId { get; set; }

        [ForeignKey("JobPostingId")]
        public JobPosting? JobPosting { get; set; }

        [Required]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;

        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    }
}