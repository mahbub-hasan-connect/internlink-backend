namespace InternLink.Api.DTOs
{
    // Module 3: Recruiter Module
    // Job posting তৈরি/আপডেট করার সময় client থেকে যা যা data আসবে
    public class CreateJobPostingRequestCJP1
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string RequiredSkills { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal? Stipend { get; set; }
    }
}