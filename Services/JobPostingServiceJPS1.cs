using InternLink.Api.DTOs;
using InternLink.Api.Models;
using InternLink.Api.Repositories;

namespace InternLink.Api.Services
{
    // Module 3: Recruiter Module
    public class JobPostingServiceJPS1 : IJobPostingServiceJPS1
    {
        private readonly IJobPostingRepositoryJPR1 _postingRepository;

        public JobPostingServiceJPS1(IJobPostingRepositoryJPR1 postingRepository)
        {
            _postingRepository = postingRepository;
        }

        public async Task<JobPostingResponseJPR1> CreatePostingAsync(int recruiterId, CreateJobPostingRequestCJP1 request)
        {
            var newPosting = new JobPosting
            {
                RecruiterId = recruiterId,
                Title = request.Title,
                Description = request.Description,
                RequiredSkills = request.RequiredSkills,
                Location = request.Location,
                Stipend = request.Stipend,
                PostedAt = DateTime.UtcNow
            };

            await _postingRepository.AddAsync(newPosting);
            await _postingRepository.SaveChangesAsync();

            return MapToResponse(newPosting);
        }

        public async Task<List<JobPostingResponseJPR1>> GetAllPostingsAsync()
        {
            var postings = await _postingRepository.GetAllAsync();
            // Select() দিয়ে প্রতিটা JobPosting-কে JobPostingResponseJPR1-এ রূপান্তর করা হচ্ছে (list-এর প্রতিটা item-এর জন্য MapToResponse কল হচ্ছে)
            return postings.Select(MapToResponse).ToList();
        }

        public async Task<List<JobPostingResponseJPR1>> GetPostingsByRecruiterAsync(int recruiterId)
        {
            var postings = await _postingRepository.GetByRecruiterIdAsync(recruiterId);
            return postings.Select(MapToResponse).ToList();
        }

        public async Task<JobPostingResponseJPR1?> GetPostingByIdAsync(int id)
        {
            var posting = await _postingRepository.GetByIdAsync(id);
            return posting == null ? null : MapToResponse(posting);
        }

        public async Task<JobPostingResponseJPR1> UpdatePostingAsync(int recruiterId, int postingId, CreateJobPostingRequestCJP1 request)
        {
            var posting = await _postingRepository.GetByIdAsync(postingId);

            if (posting == null)
            {
                throw new Exception("Job posting পাওয়া যায়নি");
            }

            // Ownership Check — এই posting-টা কি সেই Recruiter-এরই, যে update করার চেষ্টা করছে?
            if (posting.RecruiterId != recruiterId)
            {
                throw new UnauthorizedAccessException("এই posting আপডেট করার অনুমতি তোমার নেই");
            }

            posting.Title = request.Title;
            posting.Description = request.Description;
            posting.RequiredSkills = request.RequiredSkills;
            posting.Location = request.Location;
            posting.Stipend = request.Stipend;

            await _postingRepository.SaveChangesAsync();

            return MapToResponse(posting);
        }

        public async Task DeletePostingAsync(int recruiterId, int postingId)
        {
            var posting = await _postingRepository.GetByIdAsync(postingId);

            if (posting == null)
            {
                throw new Exception("Job posting পাওয়া যায়নি");
            }

            // এখানেও Ownership Check — Update-এর মতোই
            if (posting.RecruiterId != recruiterId)
            {
                throw new UnauthorizedAccessException("এই posting delete করার অনুমতি তোমার নেই");
            }

            _postingRepository.Remove(posting);
            await _postingRepository.SaveChangesAsync();
        }

        private JobPostingResponseJPR1 MapToResponse(JobPosting posting)
        {
            return new JobPostingResponseJPR1
            {
                Id = posting.Id,
                Title = posting.Title,
                Description = posting.Description,
                RequiredSkills = posting.RequiredSkills,
                Location = posting.Location,
                Stipend = posting.Stipend,
                PostedAt = posting.PostedAt
            };
        }
    }
}