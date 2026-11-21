using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternLink.Api.Models
{
    // Module 2: Student Module
    // কেন এই Model দরকার — Student-দের নিজের সম্পর্কে তথ্য (GPA, skills, resume) রাখার জন্য
    // এটা User model থেকে আলাদা কারণ User শুধু login/auth data রাখে (email, password)
    // এখানে StudentProfile Foreign Key (UserId) দিয়ে সেই User-এর সাথে যুক্ত থাকবে (এক User = এক Profile)
    public class StudentProfile
    {
        [Key]
        public int Id { get; set; }

        // Foreign Key — কোন User-এর profile এটা, সেই link
        [Required]
        public int UserId { get; set; }

        // Navigation Property — EF Core-কে বলে যে এই StudentProfile আসলে একটা User-এর সাথে সম্পর্কিত
        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        [Range(0.0, 4.0)]
        public double GPA { get; set; }

        // Comma দিয়ে আলাদা করা skill list (যেমন "C#, React, SQL") — সহজ approach, পরে আলাদা table-এও করা যায়
        public string TechStack { get; set; } = string.Empty;

        public string? PortfolioLink { get; set; }

        public string? ResumeFileUrl { get; set; }

        // Recruiter Module — direct communication (UR3): lets a recruiter call
        // an applicant, not just email/message them. Optional since a student
        // may not have filled it in yet.
        public string? Phone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}