namespace InternLink.Api.DTOs
{
    // Module 6: Application & Status Tracking
    // Student apply করার সময় শুধু JobPostingId পাঠাবে — বাকি সব (StudentUserId token থেকে, Status default) auto হবে
    public class ApplyRequestAPQ1
    {
        public int JobPostingId { get; set; }
    }
}