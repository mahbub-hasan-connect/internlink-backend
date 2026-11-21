namespace InternLink.Api.DTOs
{
    // Module 6: Application & Status Tracking
    public class ApplicationResponseAPR1
    {
        public int Id { get; set; }
        public int JobPostingId { get; set; }
        public string JobTitle { get; set; } = string.Empty; // Recruiter/Student-এর জন্য সহজে বোঝার জন্য posting title-ও দেখানো হচ্ছে
        public string Status { get; set; } = string.Empty;
        public DateTime AppliedAt { get; set; }
    }
}