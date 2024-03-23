using Authentication.API.Models.Dtos.Auth;
using Authentication.API.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto user)
        {
            var result = await _authService.UserRegisterAsync(user, Enums.Role.User);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto user)
        {
            var token = await _authService.UserLoginAsync(user);
            return Ok(new { Token = token });
        }
    }
}
