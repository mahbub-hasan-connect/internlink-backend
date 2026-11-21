namespace InternLink.Api.DTOs
{
    // Module 2: Student Module
    // Profile create/get/update করার পর client-কে যা ফেরত দেওয়া হবে
    public class StudentProfileResponseSPR1
    {
        public int Id { get; set; }
        public double GPA { get; set; }
        public string TechStack { get; set; } = string.Empty;
        public string? PortfolioLink { get; set; }
        public string? ResumeFileUrl { get; set; }
        public string? Phone { get; set; }
    }
}