namespace InternLink.Api.DTOs
{
    // Module 3: Recruiter Module
    // Job posting সম্পর্কে client-কে যা ফেরত দেওয়া হবে
    public class JobPostingResponseJPR1
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string RequiredSkills { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal? Stipend { get; set; }
        public DateTime PostedAt { get; set; }
    }
}