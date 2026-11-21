using InternLink.Api.Data;
using InternLink.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InternLink.Api.Repositories
{
    // Module 7: Notification System
    public class NotificationRepositoryNTR1 : INotificationRepositoryNTR1
    {
        private readonly AppDbContext _context;

        public NotificationRepositoryNTR1(AppDbContext context)
        {
            _context = context;
        }

        // OrderByDescending দিয়ে নতুন notification আগে দেখানো হচ্ছে (সবচেয়ে সাম্প্রতিক প্রথমে)
        public async Task<List<Notification>> GetByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<Notification?> GetByIdAsync(int id)
        {
            return await _context.Notifications.FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}