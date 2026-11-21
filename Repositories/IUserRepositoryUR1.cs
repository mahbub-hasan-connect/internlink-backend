using InternLink.Api.Models;

namespace InternLink.Api.Repositories
{
    // Repository layer শুধু database-এর সাথে কথা বলে — কোনো business logic (password hash, validation) এখানে থাকবে না
    // Interface ব্যবহার করছি যাতে পরে database engine বদলালেও Service/Controller-এর code touch করতে না হয়
    // Flow: Controller → Service → Repository → Database
    public interface IUserRepositoryUR1
    {
        // Login-এর সময় email দিয়ে user খুঁজতে ব্যবহার হবে
        Task<User?> GetByEmailAsync(string email);

        // Registration-এর সময় নতুন user database-এ যোগ করতে ব্যবহার হবে
        Task AddAsync(User user);

        // EF Core-কে বলে যে changes গুলো আসলেই database-এ save করে ফেলো
        Task SaveChangesAsync();

        // Module 4: Admin Module — সব user-এর তালিকা দেখতে ব্যবহার হবে
        Task<List<User>> GetAllAsync();

        // নির্দিষ্ট Id দিয়ে user খোঁজা — Admin কোনো specific user-কে suspend/activate করার আগে খুঁজে বের করতে ব্যবহার করবে
        Task<User?> GetByIdAsync(int id);
    }
}