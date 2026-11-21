using InternLink.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InternLink.Api.Controllers
{
    // Module 7: Notification System
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationControllerNTC1 : ControllerBase
    {
        private readonly INotificationServiceNTS1 _notificationService;

        public NotificationControllerNTC1(INotificationServiceNTS1 notificationService)
        {
            _notificationService = notificationService;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return int.Parse(userIdClaim);
        }

        // GET /api/NotificationControllerNTC1 — নিজের notification list দেখা
        [HttpGet]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = GetCurrentUserId();
            var result = await _notificationService.GetMyNotificationsAsync(userId);
            return Ok(result);
        }

        // PUT /api/NotificationControllerNTC1/5/read — notification read হিসেবে mark করা
        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            try
            {
                await _notificationService.MarkAsReadAsync(id);
                return Ok(new { message = "Notification পড়া হয়েছে বলে চিহ্নিত করা হয়েছে" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}