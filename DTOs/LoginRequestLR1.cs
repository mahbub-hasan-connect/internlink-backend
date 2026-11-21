namespace InternLink.Api.DTOs
{
    // Login করার সময় client শুধু email আর password পাঠাবে
    public class LoginRequestLR1
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}