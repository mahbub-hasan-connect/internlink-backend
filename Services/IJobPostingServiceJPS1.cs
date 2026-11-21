using InternLink.Api.DTOs;

namespace InternLink.Api.Services
{
    // Module 3: Recruiter Module
    public interface IJobPostingServiceJPS1
    {
        // নতুন posting তৈরি করে — কোন Recruiter তৈরি করছে সেটা recruiterId দিয়ে জানানো হয়
        Task<JobPostingResponseJPR1> CreatePostingAsync(int recruiterId, CreateJobPostingRequestCJP1 request);

        // সব posting দেখার জন্য (Student browse করার সময় ব্যবহার হবে)
        Task<List<JobPostingResponseJPR1>> GetAllPostingsAsync();

        // Recruiter Dashboard-এর জন্য — শুধু এই Recruiter নিজে যা যা posting করেছে তার তালিকা
        Task<List<JobPostingResponseJPR1>> GetPostingsByRecruiterAsync(int recruiterId);

        // নির্দিষ্ট Id দিয়ে একটা posting দেখা
        Task<JobPostingResponseJPR1?> GetPostingByIdAsync(int id);

        // posting আপডেট করা — শুধু সেই Recruiter করতে পারবে যে posting-টা তৈরি করেছিল (ownership check)
        Task<JobPostingResponseJPR1> UpdatePostingAsync(int recruiterId, int postingId, CreateJobPostingRequestCJP1 request);

        // posting delete করা — এখানেও ownership check লাগবে
        Task DeletePostingAsync(int recruiterId, int postingId);
    }
}