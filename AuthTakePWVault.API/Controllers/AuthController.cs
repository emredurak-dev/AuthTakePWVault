using Microsoft.AspNetCore.Mvc;
using AuthTakePWVault.API.Services;
using AuthTakePWVault.Data.Entities;

namespace AuthTakePWVault.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AuthController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userService.AuthenticateAsync(model.Username, model.Password);
            if (user == null)
                return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı!" });

            var token = _jwtService.GenerateToken(user);
            return Ok(new { token, username = user.Username });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email
                };

                await _userService.RegisterAsync(user, model.Password);
                return Ok(new { message = "Kayıt başarılı!" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
} 