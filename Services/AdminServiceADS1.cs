using InternLink.Api.DTOs;
using InternLink.Api.Repositories;

namespace InternLink.Api.Services
{
    // Module 4: Admin Module
    // লক্ষ্য করো — এখানে দুইটা আলাদা Repository constructor-এ inject হচ্ছে (আগের Service গুলোতে একটাই ছিল)
    // কারণ Admin-এর কাজ User আর JobPosting দুটোর সাথেই সম্পর্কিত
    public class AdminServiceADS1 : IAdminServiceADS1
    {
        private readonly IUserRepositoryUR1 _userRepository;
        private readonly IJobPostingRepositoryJPR1 _postingRepository;

        public AdminServiceADS1(IUserRepositoryUR1 userRepository, IJobPostingRepositoryJPR1 postingRepository)
        {
            _userRepository = userRepository;
            _postingRepository = postingRepository;
        }

        public async Task<List<UserSummaryResponseUSR1>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(u => new UserSummaryResponseUSR1
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role.ToString(),
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            }).ToList();
        }

        public async Task<UserSummaryResponseUSR1> UpdateUserStatusAsync(int userId, bool isActive)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("User পাওয়া যায়নি");
            }

            user.IsActive = isActive;
            await _userRepository.SaveChangesAsync();

            return new UserSummaryResponseUSR1
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task DeleteJobPostingAsync(int postingId)
        {
            var posting = await _postingRepository.GetByIdAsync(postingId);

            if (posting == null)
            {
                throw new Exception("Job posting পাওয়া যায়নি");
            }

            // লক্ষ্য করো — এখানে কোনো ownership check নেই (Module 3-এ Recruiter-এর জন্য ছিল)
            // কারণ Admin-এর যেকোনো posting delete করার অধিকার আছে, শুধু নিজেরটা না
            _postingRepository.Remove(posting);
            await _postingRepository.SaveChangesAsync();
        }
    }
}