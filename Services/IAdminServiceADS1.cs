using InternLink.Api.DTOs;

namespace InternLink.Api.Services
{
    // Module 4: Admin Module
    public interface IAdminServiceADS1
    {
        // সব user-এর summary list দেখা
        Task<List<UserSummaryResponseUSR1>> GetAllUsersAsync();

        // কোনো user-কে suspend/activate করা
        Task<UserSummaryResponseUSR1> UpdateUserStatusAsync(int userId, bool isActive);

        // Admin যেকোনো job posting delete করতে পারবে (ownership check ছাড়াই — Recruiter module-এর থেকে আলাদা)
        Task DeleteJobPostingAsync(int postingId);
    }
}