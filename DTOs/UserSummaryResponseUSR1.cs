namespace InternLink.Api.DTOs
{
    // Module 4: Admin Module
    // Admin panel-এ user list দেখানোর জন্য — PasswordHash-এর মতো sensitive field এখানে নেই
    public class UserSummaryResponseUSR1
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}