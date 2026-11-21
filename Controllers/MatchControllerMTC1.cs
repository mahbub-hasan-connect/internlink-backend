using InternLink.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternLink.Api.Controllers
{
    // Module 5: AI Skill-Match Engine
    // এখানে কোনো [Authorize] বসাইনি ইচ্ছাকৃতভাবে — কারণ Student নিজের match % দেখতে চাইবে,
    // ভবিষ্যতে চাইলে [Authorize(Roles = "Student")] যোগ করা যাবে সহজেই
    [ApiController]
    [Route("api/[controller]")]
    public class MatchControllerMTC1 : ControllerBase
    {
        private readonly ISkillMatchServiceSMS1 _matchService;

        public MatchControllerMTC1(ISkillMatchServiceSMS1 matchService)
        {
            _matchService = matchService;
        }

        // GET /api/MatchControllerMTC1/5/3 — Student (User Id: 5) আর Job Posting (Id: 3)-এর মধ্যে match % বের করা
        [HttpGet("{studentUserId}/{jobPostingId}")]
        public async Task<IActionResult> GetMatch(int studentUserId, int jobPostingId)
        {
            try
            {
                var result = await _matchService.CalculateMatchAsync(studentUserId, jobPostingId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}