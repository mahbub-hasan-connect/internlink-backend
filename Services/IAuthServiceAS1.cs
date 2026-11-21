using InternLink.Api.Models;

namespace InternLink.Api.Services
{
    // Auth Service-এর "contract" — এখানে business logic-এর নিয়ম বলা আছে, কিন্তু আসল implementation AuthServiceAS1.cs-এ
    public interface IAuthServiceAS1
    {
        // নতুন user register করে — email আগে থেকে আছে কিনা check করে, password hash করে, database-এ save করে
        Task<string> RegisterAsync(string name, string email, string password, UserRole role);

        // Login করার সময় email+password verify করে, ঠিক হলে JWT token return করে
        Task<string?> LoginAsync(string email, string password);
    }
}