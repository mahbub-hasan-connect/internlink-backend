using InternLink.Api.Data;
using InternLink.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InternLink.Api.Repositories
{
    // Module 2: Student Module
    // IStudentProfileRepositorySPR1-এর আসল implementation — এখানে EF Core দিয়ে সত্যিকারের database query হয়
    public class StudentProfileRepositorySPR1 : IStudentProfileRepositorySPR1
    {
        private readonly AppDbContext _context;

        public StudentProfileRepositorySPR1(AppDbContext context)
        {
            _context = context;
        }

        // UserId দিয়ে StudentProfiles table-এ খোঁজে, না পেলে null return করে
        public async Task<StudentProfile?> GetByUserIdAsync(int userId)
        {
            return await _context.StudentProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        }

        // নতুন profile-কে memory-তে track করে (এখনো database-এ যায়নি)
        public async Task AddAsync(StudentProfile profile)
        {
            await _context.StudentProfiles.AddAsync(profile);
        }

        // Database-এ আসল change save করে (add বা update দুটোর জন্যই ব্যবহার হয়)
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}