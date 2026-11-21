using InternLink.Api.DTOs;
using InternLink.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace InternLink.Api.Controllers
{
    // Module 3: Recruiter Module
    // লক্ষ্য করো — এখানে Controller-এর উপরে কোনো [Authorize] নেই (Module 2-এর মতো)
    // কারণ কিছু endpoint (GET) সবার জন্য খোলা, কিছু (POST/PUT/DELETE) শুধু Recruiter-এর জন্য
    // তাই [Authorize] প্রতিটা নির্দিষ্ট endpoint-এ আলাদাভাবে বসানো হয়েছে
    [ApiController]
    [Route("api/[controller]")]
    public class JobPostingControllerJPC1 : ControllerBase
    {
        private readonly IJobPostingServiceJPS1 _postingService;
        private readonly IApplicationServiceAPS1 _applicationService; // CSV export-এর জন্য নতুন dependency

        public JobPostingControllerJPC1(IJobPostingServiceJPS1 postingService, IApplicationServiceAPS1 applicationService)
        {
            _postingService = postingService;
            _applicationService = applicationService;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return int.Parse(userIdClaim);
        }

        // GET /api/JobPostingControllerJPC1 — সবার জন্য খোলা (Student, Recruiter, এমনকি login ছাড়াও দেখা যাবে)
        // তাই এখানে কোনো [Authorize] নেই
        [HttpGet]
        public async Task<IActionResult> GetAllPostings()
        {
            var postings = await _postingService.GetAllPostingsAsync();
            return Ok(postings);
        }

        // GET /api/JobPostingControllerJPC1/my-postings — শুধু Recruiter, নিজের posting-গুলোর তালিকা (Recruiter Dashboard)
        // এটা "{id:int}" route-এর আগে বসানো, আর id route-এ ":int" constraint যোগ করা হয়েছে —
        // নাহলে "my-postings" string-টা {id}-এর সাথে ভুলভাবে match হয়ে যেত
        [HttpGet("my-postings")]
        [Authorize(Roles = "Recruiter")]
        public async Task<IActionResult> GetMyPostings()
        {
            var recruiterId = GetCurrentUserId();
            var result = await _postingService.GetPostingsByRecruiterAsync(recruiterId);
            return Ok(result);
        }

        // GET /api/JobPostingControllerJPC1/5 — নির্দিষ্ট posting দেখা, এটাও সবার জন্য খোলা
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPostingById(int id)
        {
            var posting = await _postingService.GetPostingByIdAsync(id);

            if (posting == null)
            {
                return NotFound(new { message = "Job posting পাওয়া যায়নি" });
            }

            return Ok(posting);
        }

        // POST /api/JobPostingControllerJPC1 — শুধু Recruiter তৈরি করতে পারবে
        [HttpPost]
        [Authorize(Roles = "Recruiter")]
        public async Task<IActionResult> CreatePosting([FromBody] CreateJobPostingRequestCJP1 request)
        {
            var recruiterId = GetCurrentUserId();
            var result = await _postingService.CreatePostingAsync(recruiterId, request);
            return Ok(result);
        }

        // PUT /api/JobPostingControllerJPC1/5 — শুধু Recruiter, এবং শুধু নিজের posting update করতে পারবে
        [HttpPut("{id}")]
        [Authorize(Roles = "Recruiter")]
        public async Task<IActionResult> UpdatePosting(int id, [FromBody] CreateJobPostingRequestCJP1 request)
        {
            try
            {
                var recruiterId = GetCurrentUserId();
                var result = await _postingService.UpdatePostingAsync(recruiterId, id, request);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                // Ownership check fail করলে 403 Forbidden পাঠানো হচ্ছে (401 Unauthorized থেকে আলাদা — এখানে identity জানা আছে, কিন্তু permission নেই)
                return StatusCode(403, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE /api/JobPostingControllerJPC1/5 — শুধু Recruiter, শুধু নিজের posting delete করতে পারবে
        [HttpDelete("{id}")]
        [Authorize(Roles = "Recruiter")]
        public async Task<IActionResult> DeletePosting(int id)
        {
            try
            {
                var recruiterId = GetCurrentUserId();
                await _postingService.DeletePostingAsync(recruiterId, id);
                return Ok(new { message = "Job posting delete করা হয়েছে" });
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

        // GET /api/JobPostingControllerJPC1/5/export-csv — শুধু Recruiter, শুধু নিজের posting-এর applicant CSV export করতে পারবে
        [HttpGet("{id}/export-csv")]
        [Authorize(Roles = "Recruiter")]
        public async Task<IActionResult> ExportApplicantsCsv(int id)
        {
            try
            {
                var recruiterId = GetCurrentUserId();
                var csvContent = await _applicationService.GenerateApplicantsCsvAsync(recruiterId, id);

                // File() method দিয়ে response-কে downloadable file হিসেবে পাঠানো হচ্ছে
                var bytes = Encoding.UTF8.GetBytes(csvContent);
                return File(bytes, "text/csv", $"applicants_posting_{id}.csv");
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