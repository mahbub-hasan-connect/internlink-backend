using InternLink.Api.DTOs;
using InternLink.Api.Models;
using InternLink.Api.Repositories;

namespace InternLink.Api.Services
{
    // Module 7: Notification System
    public class NotificationServiceNTS1 : INotificationServiceNTS1
    {
        private readonly INotificationRepositoryNTR1 _notificationRepository;

        public NotificationServiceNTS1(INotificationRepositoryNTR1 notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task CreateNotificationAsync(int userId, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification);
            await _notificationRepository.SaveChangesAsync();
        }

        public async Task<List<NotificationResponseNTR1>> GetMyNotificationsAsync(int userId)
        {
            var notifications = await _notificationRepository.GetByUserIdAsync(userId);

            return notifications.Select(n => new NotificationResponseNTR1
            {
                Id = n.Id,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            }).ToList();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null)
            {
                throw new Exception("Notification পাওয়া যায়নি");
            }

            notification.IsRead = true;
            await _notificationRepository.SaveChangesAsync();
        }
    }
}