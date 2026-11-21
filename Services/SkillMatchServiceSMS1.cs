using InternLink.Api.DTOs;
using InternLink.Api.Repositories;

namespace InternLink.Api.Services
{
    // Module 5: AI Skill-Match Engine
    // এখানে StudentProfile আর JobPosting দুটো Repository-ই লাগবে — কারণ দুই দিকের skill তুলনা করতে হবে
    public class SkillMatchServiceSMS1 : ISkillMatchServiceSMS1
    {
        private readonly IStudentProfileRepositorySPR1 _profileRepository;
        private readonly IJobPostingRepositoryJPR1 _postingRepository;

        public SkillMatchServiceSMS1(IStudentProfileRepositorySPR1 profileRepository, IJobPostingRepositoryJPR1 postingRepository)
        {
            _profileRepository = profileRepository;
            _postingRepository = postingRepository;
        }

        public async Task<MatchResultResponseMRR1> CalculateMatchAsync(int studentUserId, int jobPostingId)
        {
            var profile = await _profileRepository.GetByUserIdAsync(studentUserId);
            var posting = await _postingRepository.GetByIdAsync(jobPostingId);

            if (profile == null)
            {
                throw new Exception("Student profile পাওয়া যায়নি");
            }
            if (posting == null)
            {
                throw new Exception("Job posting পাওয়া যায়নি");
            }

            // ধাপ ১: Comma-separated string ("C#, React, SQL") কে আলাদা আলাদা skill-এ ভাগ করা
            // Split(',') দিয়ে ভাগ করে, Trim() দিয়ে extra space সরিয়ে, ToLower() দিয়ে case-insensitive তুলনা করার জন্য
            var studentSkills = profile.TechStack
                .Split(',')
                .Select(s => s.Trim().ToLower())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();

            var requiredSkills = posting.RequiredSkills
                .Split(',')
                .Select(s => s.Trim().ToLower())
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();

            // ধাপ ২: কোন skill গুলো দুই লিস্টেই আছে (matching), আর কোনগুলো শুধু required-এ আছে কিন্তু student-এর নেই (missing)
            var matchingSkills = requiredSkills.Where(skill => studentSkills.Contains(skill)).ToList();
            var missingSkills = requiredSkills.Where(skill => !studentSkills.Contains(skill)).ToList();

            // ধাপ ৩: Match Percentage বের করা
            // ফর্মুলা: (কতগুলো required skill match করেছে / মোট কতগুলো required skill আছে) × 100
            double matchPercentage = requiredSkills.Count == 0
                ? 0
                : (double)matchingSkills.Count / requiredSkills.Count * 100;

            return new MatchResultResponseMRR1
            {
                MatchPercentage = Math.Round(matchPercentage, 2), // দশমিকের পর ২ ঘর পর্যন্ত round করা
                MatchingSkills = matchingSkills,
                MissingSkills = missingSkills
            };
        }
    }
}