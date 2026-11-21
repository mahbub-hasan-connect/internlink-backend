namespace InternLink.Api.DTOs
{
    // Module 4: Admin Module
    // Admin কোনো user-কে suspend/activate করার সময় এই data পাঠাবে
    public class UpdateUserStatusRequestUUS1
    {
        public bool IsActive { get; set; }
    }
}