namespace InternLink.Api.DTOs
{
    // Module 6: Application & Status Tracking
    // Recruiter application status বদলানোর সময় এই data পাঠাবে (যেমন "Shortlisted", "Interview")
    public class UpdateApplicationStatusRequestUAS1
    {
        public string Status { get; set; } = string.Empty;
    }
}