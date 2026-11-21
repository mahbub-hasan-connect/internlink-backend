using InternLink.Api.Models;

namespace InternLink.Api.DTOs
{
    // Registration form থেকে client যা যা data পাঠাবে, শুধু তাই এখানে থাকবে — User model-এর মতো PasswordHash/CreatedAt এখানে নেই
    public class RegisterRequestRR1
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // এখানে raw password আসবে, Service layer গিয়ে hash হবে
        public UserRole Role { get; set; }
    }
}