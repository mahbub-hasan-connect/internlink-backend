using InternLink.Api.Data;
using InternLink.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InternLink.Api.Repositories
{
    // Module 3: Recruiter Module
    // IJobPostingRepositoryJPR1-এর আসল implementation
    public class JobPostingRepositoryJPR1 : IJobPostingRepositoryJPR1
    {
        private readonly AppDbContext _context;

        public JobPostingRepositoryJPR1(AppDbContext context)
        {
            _context = context;
        }

        // সব posting list আকারে ফেরত দেয় — ToListAsync() ব্যবহার করা হচ্ছে কারণ একাধিক row আসতে পারে
        public async Task<List<JobPosting>> GetAllAsync()
        {
            return await _context.JobPostings.ToListAsync();
        }

        public async Task<JobPosting?> GetByIdAsync(int id)
        {
            return await _context.JobPostings.FirstOrDefaultAsync(p => p.Id == id);
        }

        // Where() দিয়ে filter করা হচ্ছে — শুধু নির্দিষ্ট Recruiter-এর posting গুলো বের করা
        public async Task<List<JobPosting>> GetByRecruiterIdAsync(int recruiterId)
        {
            return await _context.JobPostings.Where(p => p.RecruiterId == recruiterId).ToListAsync();
        }

        public async Task AddAsync(JobPosting posting)
        {
            await _context.JobPostings.AddAsync(posting);
        }

        // Remove — EF Core-কে বলে দেয় এই row-টা delete করতে হবে (SaveChangesAsync কল করলে আসলে delete হবে)
        public void Remove(JobPosting posting)
        {
            _context.JobPostings.Remove(posting);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}