namespace InternLink.Api.DTOs
{
    // Register/Login সফল হলে client-কে এই format-এ response ফেরত দেওয়া হবে
    public class AuthResponseAR1
    {
        public string Token { get; set; } = string.Empty; // JWT token — পরবর্তী request-এ এটা পাঠাতে হবে authenticated থাকতে
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}