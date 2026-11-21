using InternLink.Api.Models;

namespace InternLink.Api.Repositories
{
    // Module 3: Recruiter Module
    // One-to-Many সম্পর্কের কারণে এখানে List<JobPosting> return করার method-ও লাগবে (Module 2-এ শুধু single object ছিল)
    public interface IJobPostingRepositoryJPR1
    {
        // সব posting দেখার জন্য (Student-রাও ব্যবহার করবে browse করতে)
        Task<List<JobPosting>> GetAllAsync();

        // নির্দিষ্ট Id দিয়ে একটা posting খোঁজা
        Task<JobPosting?> GetByIdAsync(int id);

        // নির্দিষ্ট Recruiter-এর সব posting (Recruiter নিজের posting list দেখতে চাইলে)
        Task<List<JobPosting>> GetByRecruiterIdAsync(int recruiterId);

        Task AddAsync(JobPosting posting);

        // Delete করার জন্য আলাদা method লাগবে (Module 2-তে delete ছিল না)
        void Remove(JobPosting posting);

        Task SaveChangesAsync();
    }
}