using InternLink.Api.DTOs;

namespace InternLink.Api.Services
{
    // Module 6: Application & Status Tracking
    public interface IApplicationServiceAPS1
    {
        // Student apply করে — duplicate check সহ
        Task<ApplicationResponseAPR1> ApplyAsync(int studentUserId, ApplyRequestAPQ1 request);

        // Student-এর নিজের সব application দেখা
        Task<List<ApplicationResponseAPR1>> GetMyApplicationsAsync(int studentUserId);

        // Recruiter-এর posting-এ কারা apply করেছে দেখা (ownership check সহ)
        Task<List<ApplicationResponseAPR1>> GetApplicantsForPostingAsync(int recruiterId, int jobPostingId);

        // Recruiter Dashboard-এর জন্য — student-এর নাম, GPA, skills, resume link, আর AI match % সহ richer applicant view (ownership check সহ)
        Task<List<ApplicantDetailResponseADR1>> GetApplicantDetailsForPostingAsync(int recruiterId, int jobPostingId);

        // Recruiter status update করে (ownership check সহ)
        Task<ApplicationResponseAPR1> UpdateStatusAsync(int recruiterId, int applicationId, string newStatus);

        // Module: CSV Export — Recruiter তার posting-এর applicant list CSV আকারে download করতে পারবে
        Task<string> GenerateApplicantsCsvAsync(int recruiterId, int jobPostingId);

        // Recruiter → Applicant direct communication — sends a custom message as an
        // in-app Notification to the student (ownership check সহ)
        Task SendMessageToApplicantAsync(int recruiterId, int applicationId, string message);
    }
}