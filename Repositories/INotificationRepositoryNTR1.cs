using InternLink.Api.Models;

namespace InternLink.Api.Repositories
{
    // Module 7: Notification System
    public interface INotificationRepositoryNTR1
    {
        // নির্দিষ্ট User-এর সব notification দেখতে
        Task<List<Notification>> GetByUserIdAsync(int userId);

        // Read হিসেবে mark করার সময় notification খুঁজতে
        Task<Notification?> GetByIdAsync(int id);

        // Module 6-এর UpdateStatus থেকে কল হবে, নতুন notification তৈরি করতে
        Task AddAsync(Notification notification);

        Task SaveChangesAsync();
    }
}