using InternLink.Api.DTOs;
using InternLink.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InternLink.Api.Controllers
{
    // Module 6: Application & Status Tracking
    // Mixed authorization — Apply/MyApplications শুধু Student, GetApplicants/UpdateStatus শুধু Recruiter
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationControllerAPC1 : ControllerBase
    {
        private readonly IApplicationServiceAPS1 _applicationService;

        public ApplicationControllerAPC1(IApplicationServiceAPS1 applicationService)
        {
            _applicationService = applicationService;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return int.Parse(userIdClaim);
        }

        // POST /api/ApplicationControllerAPC1 — Student apply করবে
        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Apply([FromBody] ApplyRequestAPQ1 request)
        {
            try
            {
                var studentUserId = GetCurrentUserId();
                var result = await _applicationService.ApplyAsync(studentUserId, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET /api/ApplicationControllerAPC1/my-applications — Student নিজের সব application দেখবে
        [HttpGet("my-applications")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyApplications()
        {
            var studentUserId = GetCurrentUserId();
            var result = await _applicationService.GetMyApplicationsAsync(studentUserId);
            return Ok(result);
        }

        // GET /api/ApplicationControllerAPC1/posting/5 — Recruiter দেখবে কারা তার posting-এ apply করেছে
        [HttpGet("posting/{jobPostingId}")]
        [Authorize(Roles = "Recruiter")]
        public async Task<IActionResult> GetApplicantsForPosting(int jobPostingId)
        {
            try
            {
                var recruiterId = GetCurrentUserId();
                var result = await _applicationService.GetApplicantsForPostingAsync(recruiterId, jobPostingId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET /api/ApplicationControllerAPC1/posting/5/details — Recruiter Dashboard-এর জন্য richer view:
        // student-এর নাম/GPA/skills/resume link আর AI match % সহ, match % অনুযায়ী sort করা (ownership check সহ)
        [HttpGet("posting/{jobPostingId}/details")]
        [Authorize(Roles = "Recruiter")]
        public async Task<IActionResult> GetApplicantDetailsForPosting(int jobPostingId)
        {
            try
            {
                var recruiterId = GetCurrentUserId();
                var result = await _applicationService.GetApplicantDetailsForPostingAsync(recruiterId, jobPostingId);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT /api/ApplicationControllerAPC1/5/status — Recruiter status বদলাবে
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Recruiter")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateApplicationStatusRequestUAS1 request)
        {
            try
            {
                var recruiterId = GetCurrentUserId();
                var result = await _applicationService.UpdateStatusAsync(recruiterId, id, request.Status);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST /api/ApplicationControllerAPC1/5/message — Recruiter সরাসরি applicant-কে
        // message পাঠাবে (email/phone ছাড়াও in-app communication option, UR3)
        [HttpPost("{id}/message")]
        [Authorize(Roles = "Recruiter")]
        public async Task<IActionResult> SendMessageToApplicant(int id, [FromBody] SendMessageRequestSMR1 request)
        {
            try
            {
                var recruiterId = GetCurrentUserId();
                await _applicationService.SendMessageToApplicantAsync(recruiterId, id, request.Message);
                return Ok(new { message = "Message sent" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}