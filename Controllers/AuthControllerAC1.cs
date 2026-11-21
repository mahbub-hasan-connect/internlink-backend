using InternLink.Api.DTOs;
using InternLink.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace InternLink.Api.Controllers
{
    // Auth-সম্পর্কিত সব API endpoint এখানে — register, login
    // এই Controller নিজে কোনো business logic করে না, শুধু IAuthServiceAS1-কে কল করে আর HTTP response পাঠায়
    [ApiController]
    [Route("api/[controller]")]
    public class AuthControllerAC1 : ControllerBase
    {
        private readonly IAuthServiceAS1 _authService;

        public AuthControllerAC1(IAuthServiceAS1 authService)
        {
            _authService = authService;
        }

        // POST /api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestRR1 request)
        {
            try
            {
                var token = await _authService.RegisterAsync(
                    request.Name,
                    request.Email,
                    request.Password,
                    request.Role
                );

                var response = new AuthResponseAR1
                {
                    Token = token,
                    Name = request.Name,
                    Email = request.Email,
                    Role = request.Role.ToString()
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                // যেমন "email আগে থেকেই আছে" এই error এখানে ধরা পড়বে
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestLR1 request)
        {
            var token = await _authService.LoginAsync(request.Email, request.Password);

            if (token == null)
            {
                return Unauthorized(new { message = "Email অথবা password ভুল" });
            }

            return Ok(new { token });
        }
    }
}