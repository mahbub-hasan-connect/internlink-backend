namespace InternLink.Api.DTOs
{
    // Module 5: AI Skill-Match Engine
    // Match calculation-এর ফলাফল client-কে এই format-এ ফেরত দেওয়া হবে
    public class MatchResultResponseMRR1
    {
        // 0 থেকে 100-এর মধ্যে percentage — কতটা মিল আছে student-এর skill আর job-এর required skill-এর মধ্যে
        public double MatchPercentage { get; set; }

        // Student-এর যেই skill গুলো job posting-এ required, সেগুলোর তালিকা
        public List<string> MatchingSkills { get; set; } = new List<string>();

        // Job posting-এ required কিন্তু student-এর নেই, এমন skill-এর তালিকা (Skill Gap Analysis)
        public List<string> MissingSkills { get; set; } = new List<string>();
    }
}