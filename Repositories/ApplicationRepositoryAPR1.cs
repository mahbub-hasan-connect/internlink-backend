using InternLink.Api.Data;
using InternLink.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace InternLink.Api.Repositories
{
    // Module 6: Application & Status Tracking
    public class ApplicationRepositoryAPR1 : IApplicationRepositoryAPR1
    {
        private readonly AppDbContext _context;

        public ApplicationRepositoryAPR1(AppDbContext context)
        {
            _context = context;
        }

        // StudentUserId এবং JobPostingId দুটো condition একসাথে মিলিয়ে খোঁজা — duplicate check-এর জন্য
        public async Task<Application?> GetByStudentAndPostingAsync(int studentUserId, int jobPostingId)
        {
            return await _context.Applications
                .FirstOrDefaultAsync(a => a.StudentUserId == studentUserId && a.JobPostingId == jobPostingId);
        }

        public async Task<List<Application>> GetByStudentIdAsync(int studentUserId)
        {
            return await _context.Applications
                .Where(a => a.StudentUserId == studentUserId)
                .ToListAsync();
        }

        // .Include(a => a.Student) দিয়ে User (Student) data-ও একসাথে load করা হচ্ছে
        // এটা CSV Export এবং GetApplicantsForPosting দুটোতেই দরকার — Student-এর নাম/email দেখানোর জন্য
        public async Task<List<Application>> GetByJobPostingIdAsync(int jobPostingId)
        {
            return await _context.Applications
                .Include(a => a.Student)
                .Where(a => a.JobPostingId == jobPostingId)
                .ToListAsync();
        }

        public async Task<Application?> GetByIdAsync(int id)
        {
            return await _context.Applications.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Application application)
        {
            await _context.Applications.AddAsync(application);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}