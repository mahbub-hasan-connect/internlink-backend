using InternLink.Api.DTOs;
using InternLink.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InternLink.Api.Controllers
{
    // Module 2: Student Module
    // [Authorize] মানে — এই Controller-এর সব endpoint শুধু valid JWT token থাকা user-ই call করতে পারবে
    // Roles = "Student" মানে — শুধু Student role-এর user-ই access পাবে, Recruiter/Admin না
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Student")]
    public class StudentProfileControllerSPC1 : ControllerBase
    {
        private readonly IStudentProfileServiceSPS1 _profileService;

        public StudentProfileControllerSPC1(IStudentProfileServiceSPS1 profileService)
        {
            _profileService = profileService;
        }

        // JWT token-এর ভেতর থেকে বর্তমান logged-in user-এর Id বের করার helper method
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return int.Parse(userIdClaim);
        }

        // POST /api/StudentProfileControllerSPC1 — নতুন profile তৈরি
        [HttpPost]
        public async Task<IActionResult> CreateProfile([FromBody] CreateProfileRequestCPR1 request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _profileService.CreateProfileAsync(userId, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET /api/StudentProfileControllerSPC1 — নিজের profile দেখা
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetCurrentUserId();
            var result = await _profileService.GetProfileAsync(userId);

            if (result == null)
            {
                return NotFound(new { message = "এখনো কোনো profile তৈরি করা হয়নি" });
            }

            return Ok(result);
        }

        // PUT /api/StudentProfileControllerSPC1 — profile আপডেট
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] CreateProfileRequestCPR1 request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _profileService.UpdateProfileAsync(userId, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}