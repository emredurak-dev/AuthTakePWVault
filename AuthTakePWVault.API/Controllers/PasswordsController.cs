using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AuthTakePWVault.API.Services;
using AuthTakePWVault.Data.Entities;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace AuthTakePWVault.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordsController : ControllerBase
    {
        private readonly IPasswordVaultService _passwordVaultService;

        public PasswordsController(IPasswordVaultService passwordVaultService)
        {
            _passwordVaultService = passwordVaultService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            return int.Parse(userIdClaim);
        }

        [HttpGet]
        public async Task<IActionResult> GetPasswords()
        {
            var userId = GetUserId();
            var passwords = await _passwordVaultService.GetUserPasswordsAsync(userId);
            return Ok(passwords);
        }

        [HttpPost]
        public async Task<IActionResult> AddPassword([FromBody] PasswordVault password)
        {
            var userId = GetUserId();
            password.UserId = userId;

            var result = await _passwordVaultService.AddPasswordAsync(password);
            if (result == null)
                return BadRequest(new { message = "Şifre eklenemedi!" });

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePassword(int id, [FromBody] PasswordVault password)
        {
            var userId = GetUserId();
            password.UserId = userId;
            password.Id = id;

            var result = await _passwordVaultService.UpdatePasswordAsync(password);
            if (result == null)
                return BadRequest(new { message = "Şifre güncellenemedi!" });

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassword(int id)
        {
            var userId = GetUserId();
            var result = await _passwordVaultService.DeletePasswordAsync(id, userId);
            
            if (!result)
                return BadRequest(new { message = "Şifre silinemedi!" });

            return Ok(new { message = "Şifre başarıyla silindi." });
        }
    }
} 