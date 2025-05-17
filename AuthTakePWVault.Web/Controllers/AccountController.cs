using AuthTakePWVault.API.Services;
using AuthTakePWVault.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AuthTakePWVault.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userService.AuthenticateAsync(username, password);
            if (user == null)
                return Json(new { success = false, message = "Kullanıcı adı veya şifre hatalı!" });

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);

            return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user, string password)
        {
            try
            {
                await _userService.RegisterAsync(user, password);
                return Json(new { success = true, message = "Kayıt başarılı! Giriş yapabilirsiniz." });
            }
            catch (System.ArgumentException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckUsername(string username)
        {
            var isUnique = await _userService.IsUsernameUniqueAsync(username);
            return Json(isUnique);
        }

        [HttpPost]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var isUnique = await _userService.IsEmailUniqueAsync(email);
            return Json(isUnique);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
} 