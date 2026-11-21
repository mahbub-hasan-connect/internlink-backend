using InternLink.Api.DTOs;

namespace InternLink.Api.Services
{
    // Module 5: AI Skill-Match Engine
    // এখানে কোনো database table নেই — এই Service শুধু calculation করে, StudentProfile আর JobPosting-এর existing data ব্যবহার করে
    public interface ISkillMatchServiceSMS1
    {
        // নির্দিষ্ট Student আর JobPosting-এর মধ্যে match percentage বের করে
        Task<MatchResultResponseMRR1> CalculateMatchAsync(int studentUserId, int jobPostingId);
    }
}