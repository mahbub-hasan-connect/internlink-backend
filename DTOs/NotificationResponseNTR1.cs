namespace InternLink.Api.DTOs
{
    // Module 7: Notification System
    public class NotificationResponseNTR1
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}