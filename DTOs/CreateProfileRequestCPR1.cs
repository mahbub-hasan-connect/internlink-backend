namespace InternLink.Api.DTOs
{
    // Module 2: Student Module
    // Profile তৈরি/আপডেট করার সময় client (frontend) থেকে যা যা data আসবে
    public class CreateProfileRequestCPR1
    {
        public double GPA { get; set; }
        public string TechStack { get; set; } = string.Empty;
        public string? PortfolioLink { get; set; }
        public string? ResumeFileUrl { get; set; }
        public string? Phone { get; set; }
    }
}