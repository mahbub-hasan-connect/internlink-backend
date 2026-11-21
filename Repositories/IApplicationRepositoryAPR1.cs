using InternLink.Api.Models;

namespace InternLink.Api.Repositories
{
    // Module 6: Application & Status Tracking
    public interface IApplicationRepositoryAPR1
    {
        // Duplicate application check করতে ব্যবহার হবে — একই student একই posting-এ আগে apply করেছে কিনা
        Task<Application?> GetByStudentAndPostingAsync(int studentUserId, int jobPostingId);

        // Student-এর নিজের সব application দেখতে
        Task<List<Application>> GetByStudentIdAsync(int studentUserId);

        // Recruiter-এর posting-এ কারা apply করেছে দেখতে
        Task<List<Application>> GetByJobPostingIdAsync(int jobPostingId);

        // নির্দিষ্ট Id দিয়ে একটা application খোঁজা (status update করার সময় লাগবে)
        Task<Application?> GetByIdAsync(int id);

        Task AddAsync(Application application);
        Task SaveChangesAsync();
    }
}