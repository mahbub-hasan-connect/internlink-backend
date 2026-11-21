using InternLink.Api.DTOs;
using InternLink.Api.Models;
using InternLink.Api.Repositories;

namespace InternLink.Api.Services
{
    // Module 2: Student Module
    // IStudentProfileServiceSPS1-এর আসল implementation — এখানে validation আর business logic থাকে
    public class StudentProfileServiceSPS1 : IStudentProfileServiceSPS1
    {
        private readonly IStudentProfileRepositorySPR1 _profileRepository;

        public StudentProfileServiceSPS1(IStudentProfileRepositorySPR1 profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public async Task<StudentProfileResponseSPR1> CreateProfileAsync(int userId, CreateProfileRequestCPR1 request)
        {
            // একই user-এর জন্য আগে থেকে profile আছে কিনা check করা — duplicate profile ঠেকাতে
            var existingProfile = await _profileRepository.GetByUserIdAsync(userId);
            if (existingProfile != null)
            {
                throw new Exception("এই user-এর জন্য profile আগে থেকেই তৈরি করা আছে");
            }

            var newProfile = new StudentProfile
            {
                UserId = userId,
                GPA = request.GPA,
                TechStack = request.TechStack,
                PortfolioLink = request.PortfolioLink,
                ResumeFileUrl = request.ResumeFileUrl,
                Phone = request.Phone,
                CreatedAt = DateTime.UtcNow
            };

            await _profileRepository.AddAsync(newProfile);
            await _profileRepository.SaveChangesAsync();

            return MapToResponse(newProfile);
        }

        public async Task<StudentProfileResponseSPR1?> GetProfileAsync(int userId)
        {
            var profile = await _profileRepository.GetByUserIdAsync(userId);
            if (profile == null)
            {
                return null; // Controller-এ এটা 404 Not Found হিসেবে handle হবে
            }

            return MapToResponse(profile);
        }

        public async Task<StudentProfileResponseSPR1> UpdateProfileAsync(int userId, CreateProfileRequestCPR1 request)
        {
            var profile = await _profileRepository.GetByUserIdAsync(userId);
            if (profile == null)
            {
                throw new Exception("Profile পাওয়া যায়নি — আগে profile তৈরি করতে হবে");
            }

            // Existing profile-এর field গুলো নতুন data দিয়ে replace করা হচ্ছে
            profile.GPA = request.GPA;
            profile.TechStack = request.TechStack;
            profile.PortfolioLink = request.PortfolioLink;
            profile.ResumeFileUrl = request.ResumeFileUrl;
            profile.Phone = request.Phone;

            await _profileRepository.SaveChangesAsync();

            return MapToResponse(profile);
        }

        // একই mapping logic বারবার লেখা এড়াতে একটা helper method — StudentProfile থেকে Response DTO বানায়
        private StudentProfileResponseSPR1 MapToResponse(StudentProfile profile)
        {
            return new StudentProfileResponseSPR1
            {
                Id = profile.Id,
                GPA = profile.GPA,
                TechStack = profile.TechStack,
                PortfolioLink = profile.PortfolioLink,
                ResumeFileUrl = profile.ResumeFileUrl,
                Phone = profile.Phone
            };
        }
    }
}