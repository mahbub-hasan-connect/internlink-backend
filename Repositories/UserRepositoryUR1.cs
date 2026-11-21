using InternLink.Api.Data;
using InternLink.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InternLink.Api.Repositories
{
    // IUserRepositoryUR1 interface-এর আসল implementation — এখানেই EF Core দিয়ে সত্যিকারের database query হয়
    public class UserRepositoryUR1 : IUserRepositoryUR1
    {
        private readonly AppDbContext _context;

        // Constructor Injection — AppDbContext কে "inject" করা হচ্ছে, যাতে এই class সরাসরি database connect না করে,
        // বরং Program.cs-এ যেই AppDbContext register করা আছে সেটাই ব্যবহার করে
        public UserRepositoryUR1(AppDbContext context)
        {
            _context = context;
        }

        // Users table-এ email match করে একটা user খোঁজে, না পেলে null return করে
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        // নতুন user-কে memory-তে "track" করে (এখনো database-এ যায়নি, শুধু EF Core মনে রাখে)
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        // এখানেই আসলে database-এ change গুলো commit/save হয়
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // Module 4: Admin Module — সব user list আকারে ফেরত দেয়
        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        // নির্দিষ্ট Id দিয়ে user খুঁজে বের করে
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}