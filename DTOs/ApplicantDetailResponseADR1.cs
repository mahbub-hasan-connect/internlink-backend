namespace InternLink.Api.DTOs
{
    // Module 3/5/6: Recruiter Dashboard — Applicant Review
    // GetApplicantsForPostingAsync (ApplicationResponseAPR1) শুধু application/status data দেয়, student কে সেটা বলে না।
    // Recruiter-দের candidate screen করতে student-এর নাম, GPA, skills, resume link, আর AI match % দরকার —
    // এই DTO সেগুলো একসাথে দেয়, যাতে Recruiter Dashboard-এ ranking আর review একবারেই করা যায়।
    public class ApplicantDetailResponseADR1
    {
        public int ApplicationId { get; set; }
        public int StudentUserId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public string? StudentPhone { get; set; }
        public double GPA { get; set; }
        public string TechStack { get; set; } = string.Empty;
        public string? ResumeFileUrl { get; set; }
        public double MatchPercentage { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime AppliedAt { get; set; }
    }
}
