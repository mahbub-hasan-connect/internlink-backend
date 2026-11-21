using System.ComponentModel.DataAnnotations;

namespace InternLink.Api.Models
{
    public enum UserRole
    {
        Student,
        Recruiter,
        Admin
    }

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; }

        public bool IsEmailVerified { get; set; } = false;

        // Module 4: Admin Module — Admin এই field দিয়ে কোনো user-কে suspend/activate করতে পারবে
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}