using InternLink.Api.DTOs;
using InternLink.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InternLink.Api.Controllers
{
    // Module 4: Admin Module
    // পুরো Controller-ই [Authorize(Roles = "Admin")] দিয়ে protected — শুধু Admin role-এর user এই সব endpoint access করতে পারবে
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminControllerADC1 : ControllerBase
    {
        private readonly IAdminServiceADS1 _adminService;

        public AdminControllerADC1(IAdminServiceADS1 adminService)
        {
            _adminService = adminService;
        }

        // GET /api/AdminControllerADC1/users — সব user-এর তালিকা
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            return Ok(users);
        }

        // PUT /api/AdminControllerADC1/users/5/status — কোনো user suspend/activate করা
        [HttpPut("users/{id}/status")]
        public async Task<IActionResult> UpdateUserStatus(int id, [FromBody] UpdateUserStatusRequestUUS1 request)
        {
            try
            {
                var result = await _adminService.UpdateUserStatusAsync(id, request.IsActive);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE /api/AdminControllerADC1/jobposting/5 — Admin যেকোনো posting মুছতে পারবে
        [HttpDelete("jobposting/{id}")]
        public async Task<IActionResult> DeleteJobPosting(int id)
        {
            try
            {
                await _adminService.DeleteJobPostingAsync(id);
                return Ok(new { message = "Job posting delete করা হয়েছে (Admin action)" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}