namespace InternLink.Api.Services
{
    // Module 7: Notification System
    public interface INotificationServiceNTS1
    {
        // Module 6 (Application status update) থেকে কল হবে — নতুন notification তৈরি করে
        Task CreateNotificationAsync(int userId, string message);

        // User নিজের notification list দেখতে
        Task<List<DTOs.NotificationResponseNTR1>> GetMyNotificationsAsync(int userId);

        // Notification-কে read হিসেবে mark করতে
        Task MarkAsReadAsync(int notificationId);
    }
}