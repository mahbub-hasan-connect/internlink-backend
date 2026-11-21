using InternLink.Api.DTOs;
using InternLink.Api.Models;
using InternLink.Api.Repositories;
using InternLink.Api.Services;

namespace InternLink.Api.Services
{
    // Module 6: Application & Status Tracking
    // এখানে তিনটা Repository লাগছে — Application নিজের জন্য, JobPosting ownership check করার জন্য
    // এবং NotificationServiceNTS1 — Module 7-এর সাথে সংযোগ করার জন্য (Service-to-Service dependency)
    public class ApplicationServiceAPS1 : IApplicationServiceAPS1
    {
        private readonly IApplicationRepositoryAPR1 _applicationRepository;
        private readonly IJobPostingRepositoryJPR1 _postingRepository;
        private readonly INotificationServiceNTS1 _notificationService;
        private readonly IStudentProfileRepositorySPR1 _profileRepository; // Recruiter Dashboard applicant details-এর জন্য (GPA/TechStack)
        private readonly ISkillMatchServiceSMS1 _matchService; // Recruiter Dashboard-এ প্রতিটা applicant-এর AI match % দেখানোর জন্য

        public ApplicationServiceAPS1(
            IApplicationRepositoryAPR1 applicationRepository,
            IJobPostingRepositoryJPR1 postingRepository,
            INotificationServiceNTS1 notificationService,
            IStudentProfileRepositorySPR1 profileRepository,
            ISkillMatchServiceSMS1 matchService)
        {
            _applicationRepository = applicationRepository;
            _postingRepository = postingRepository;
            _notificationService = notificationService;
            _profileRepository = profileRepository;
            _matchService = matchService;
        }

        public async Task<ApplicationResponseAPR1> ApplyAsync(int studentUserId, ApplyRequestAPQ1 request)
        {
            // ধাপ ১: Job posting আসলেই আছে কিনা check করা
            var posting = await _postingRepository.GetByIdAsync(request.JobPostingId);
            if (posting == null)
            {
                throw new Exception("Job posting not found");
            }

            // ধাপ ২: Duplicate Check — একই student একই posting-এ আগে apply করেছে কিনা
            var existingApplication = await _applicationRepository.GetByStudentAndPostingAsync(studentUserId, request.JobPostingId);
            if (existingApplication != null)
            {
                throw new Exception("You have already applied to this posting");
            }

            var newApplication = new Application
            {
                StudentUserId = studentUserId,
                JobPostingId = request.JobPostingId,
                Status = ApplicationStatus.Applied,
                AppliedAt = DateTime.UtcNow
            };

            await _applicationRepository.AddAsync(newApplication);
            await _applicationRepository.SaveChangesAsync();

            return new ApplicationResponseAPR1
            {
                Id = newApplication.Id,
                JobPostingId = posting.Id,
                JobTitle = posting.Title,
                Status = newApplication.Status.ToString(),
                AppliedAt = newApplication.AppliedAt
            };
        }

        public async Task<List<ApplicationResponseAPR1>> GetMyApplicationsAsync(int studentUserId)
        {
            var applications = await _applicationRepository.GetByStudentIdAsync(studentUserId);
            var result = new List<ApplicationResponseAPR1>();

            // প্রতিটা application-এর জন্য posting title আনতে হবে, তাই loop-এর ভেতরে posting খোঁজা হচ্ছে
            foreach (var app in applications)
            {
                var posting = await _postingRepository.GetByIdAsync(app.JobPostingId);
                result.Add(new ApplicationResponseAPR1
                {
                    Id = app.Id,
                    JobPostingId = app.JobPostingId,
                    JobTitle = posting?.Title ?? "Unknown", // posting delete হয়ে গেলেও crash না করে "Unknown" দেখাবে
                    Status = app.Status.ToString(),
                    AppliedAt = app.AppliedAt
                });
            }

            return result;
        }

        public async Task<List<ApplicationResponseAPR1>> GetApplicantsForPostingAsync(int recruiterId, int jobPostingId)
        {
            var posting = await _postingRepository.GetByIdAsync(jobPostingId);
            if (posting == null)
            {
                throw new Exception("Job posting not found");
            }

            // Ownership Check — এই posting কি এই Recruiter-এরই
            if (posting.RecruiterId != recruiterId)
            {
                throw new UnauthorizedAccessException("You do not have permission to view applicants for this posting");
            }

            var applications = await _applicationRepository.GetByJobPostingIdAsync(jobPostingId);

            return applications.Select(app => new ApplicationResponseAPR1
            {
                Id = app.Id,
                JobPostingId = app.JobPostingId,
                JobTitle = posting.Title,
                Status = app.Status.ToString(),
                AppliedAt = app.AppliedAt
            }).ToList();
        }

        // Recruiter Dashboard-এর জন্য richer view — GetApplicantsForPostingAsync-এর মতোই ownership check,
        // কিন্তু প্রতিটা applicant-এর সাথে profile (GPA/TechStack/Resume) আর AI match % ও জুড়ে দেওয়া হচ্ছে,
        // যাতে Recruiter এক পলকে সবচেয়ে ভালো candidate চিনতে পারে (proposal-এর "AI Candidate Ranking")
        public async Task<List<ApplicantDetailResponseADR1>> GetApplicantDetailsForPostingAsync(int recruiterId, int jobPostingId)
        {
            var posting = await _postingRepository.GetByIdAsync(jobPostingId);
            if (posting == null)
            {
                throw new Exception("Job posting not found");
            }

            if (posting.RecruiterId != recruiterId)
            {
                throw new UnauthorizedAccessException("You do not have permission to view applicants for this posting");
            }

            var applications = await _applicationRepository.GetByJobPostingIdAsync(jobPostingId);
            var result = new List<ApplicantDetailResponseADR1>();

            foreach (var app in applications)
            {
                var profile = await _profileRepository.GetByUserIdAsync(app.StudentUserId);

                // Match % বের করতে StudentProfile লাগে — profile না থাকলে (student এখনো profile বানায়নি) 0% দেখানো হবে, crash না করে
                double matchPercentage = 0;
                if (profile != null)
                {
                    try
                    {
                        var matchResult = await _matchService.CalculateMatchAsync(app.StudentUserId, jobPostingId);
                        matchPercentage = matchResult.MatchPercentage;
                    }
                    catch
                    {
                        matchPercentage = 0;
                    }
                }

                result.Add(new ApplicantDetailResponseADR1
                {
                    ApplicationId = app.Id,
                    StudentUserId = app.StudentUserId,
                    StudentName = app.Student?.Name ?? "Unknown",
                    StudentEmail = app.Student?.Email ?? "Unknown",
                    StudentPhone = profile?.Phone,
                    GPA = profile?.GPA ?? 0,
                    TechStack = profile?.TechStack ?? string.Empty,
                    ResumeFileUrl = profile?.ResumeFileUrl,
                    MatchPercentage = matchPercentage,
                    Status = app.Status.ToString(),
                    AppliedAt = app.AppliedAt
                });
            }

            // সবচেয়ে বেশি match % আগে — Recruiter সবার আগে সবচেয়ে ভালো candidate দেখবে
            return result.OrderByDescending(a => a.MatchPercentage).ToList();
        }

        public async Task<ApplicationResponseAPR1> UpdateStatusAsync(int recruiterId, int applicationId, string newStatus)
        {
            var application = await _applicationRepository.GetByIdAsync(applicationId);
            if (application == null)
            {
                throw new Exception("Application not found");
            }

            var posting = await _postingRepository.GetByIdAsync(application.JobPostingId);
            if (posting == null || posting.RecruiterId != recruiterId)
            {
                throw new UnauthorizedAccessException("You do not have permission to change the status of this application");
            }

            // String ("Shortlisted") থেকে Enum (ApplicationStatus.Shortlisted)-এ রূপান্তর
            // Enum.TryParse ব্যর্থ হলে false return করে, exception ছোঁড়ে না — তাই নিজে check করে exception ছুঁড়ছি
            if (!Enum.TryParse<ApplicationStatus>(newStatus, true, out var parsedStatus))
            {
                throw new Exception("Invalid status. Valid values: Applied, Shortlisted, Interview, Offered, Hired, Rejected");
            }

            application.Status = parsedStatus;
            await _applicationRepository.SaveChangesAsync();

            // Module 7: Status বদলানোর সাথে সাথে Student-কে notification পাঠানো হচ্ছে
            // (Notification content is always English — the UI itself stays bilingual-friendly
            // via Bangla code comments, but user-facing text is English per project requirement)
            var message = $"Your application status for '{posting.Title}' has been updated to '{parsedStatus}'";
            await _notificationService.CreateNotificationAsync(application.StudentUserId, message);

            return new ApplicationResponseAPR1
            {
                Id = application.Id,
                JobPostingId = application.JobPostingId,
                JobTitle = posting.Title,
                Status = application.Status.ToString(),
                AppliedAt = application.AppliedAt
            };
        }

        // Module: CSV Export
        public async Task<string> GenerateApplicantsCsvAsync(int recruiterId, int jobPostingId)
        {
            var posting = await _postingRepository.GetByIdAsync(jobPostingId);
            if (posting == null)
            {
                throw new Exception("Job posting not found");
            }

            // Ownership Check — শুধু posting-এর owner Recruiter এটা export করতে পারবে
            if (posting.RecruiterId != recruiterId)
            {
                throw new UnauthorizedAccessException("You do not have permission to export applicants for this posting");
            }

            var applications = await _applicationRepository.GetByJobPostingIdAsync(jobPostingId);

            // CSV বানানোর জন্য StringBuilder ব্যবহার করা হচ্ছে — প্রতিটা লাইন comma দিয়ে আলাদা করা column
            var csv = new System.Text.StringBuilder();

            // ধাপ ১: Header row (column-এর নাম)
            csv.AppendLine("ApplicationId,StudentName,StudentEmail,Status,AppliedDate");

            // ধাপ ২: প্রতিটা application-এর জন্য একটা row — Student-এর তথ্য আনতে হবে User থেকে
            foreach (var app in applications)
            {
                // Application.Student navigation property ব্যবহার করা হচ্ছে (User model-এর সাথে সংযুক্ত)
                var studentName = app.Student?.Name ?? "Unknown";
                var studentEmail = app.Student?.Email ?? "Unknown";

                csv.AppendLine($"{app.Id},{studentName},{studentEmail},{app.Status},{app.AppliedAt:yyyy-MM-dd}");
            }

            return csv.ToString();
        }

        // Recruiter → Applicant direct communication (UR3 "direct communication" requirement).
        // Reuses the existing Notification system rather than a brand new messaging table —
        // the message shows up for the student on their Notifications page.
        public async Task SendMessageToApplicantAsync(int recruiterId, int applicationId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new Exception("Message cannot be empty");
            }

            var application = await _applicationRepository.GetByIdAsync(applicationId);
            if (application == null)
            {
                throw new Exception("Application not found");
            }

            var posting = await _postingRepository.GetByIdAsync(application.JobPostingId);
            if (posting == null || posting.RecruiterId != recruiterId)
            {
                throw new UnauthorizedAccessException("You do not have permission to message this applicant");
            }

            var fullMessage = $"Message from the recruiter regarding '{posting.Title}': {message}";
            await _notificationService.CreateNotificationAsync(application.StudentUserId, fullMessage);
        }
    }
}