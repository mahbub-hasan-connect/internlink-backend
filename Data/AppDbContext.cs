

using Microsoft.EntityFrameworkCore;
using InternLink.Api.Models;

namespace InternLink.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        // StudentProfile table-ও EF Core-কে জানাতে হবে, নাহলে migration এটা database-এ বানাবে না
        public DbSet<StudentProfile> StudentProfiles { get; set; }

        // Module 3: Recruiter Module — JobPosting table
        public DbSet<JobPosting> JobPostings { get; set; }

        // Module 6: Application & Status Tracking
        public DbSet<Application> Applications { get; set; }

        // Module 7: Notification System
        public DbSet<Notification> Notifications { get; set; }
    }
}

