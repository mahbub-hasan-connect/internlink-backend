using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternLink.Api.Models
{
    // Module 3: Recruiter Module
    // এই Model-এ Recruiter-দের পোস্ট করা internship/job-এর তথ্য থাকবে
    // এক Recruiter (User) একাধিক JobPosting করতে পারবে — One-to-Many relationship (Module 2-এর One-to-One থেকে আলাদা)
    public class JobPosting
    {
        [Key]
        public int Id { get; set; }

        // Foreign Key — কোন Recruiter (User) এই posting-টা করেছে
        [Required]
        public int RecruiterId { get; set; }

        // Navigation Property — এই posting-টা কোন User-এর সাথে যুক্ত সেটা সরাসরি access করার জন্য
        [ForeignKey("RecruiterId")]
        public User? Recruiter { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        // Comma দিয়ে আলাদা করা skill list — Module 2-এর TechStack-এর মতোই approach, পরে AI Matching (Module 5) এখানেই compare করবে
        [Required]
        public string RequiredSkills { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public decimal? Stipend { get; set; }

        public DateTime PostedAt { get; set; } = DateTime.UtcNow;
    }
}