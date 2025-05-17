using System.Diagnostics;
using AuthTakePWVault.Web.Models;
using Microsoft.AspNetCore.Mvc;
using AuthTakePWVault.API.Services;
using AuthTakePWVault.Data.Entities;
using System.Threading.Tasks;

namespace AuthTakePWVault.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPasswordVaultService _passwordVaultService;

        public HomeController(ILogger<HomeController> logger, IPasswordVaultService passwordVaultService)
        {
            _logger = logger;
            _passwordVaultService = passwordVaultService;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPasswords()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return Json(new { success = false, message = "Oturum süresi dolmuş!" });

            var passwords = await _passwordVaultService.GetUserPasswordsAsync(userId.Value);
            return Json(new { success = true, data = passwords });
        }

        [HttpPost]
        public async Task<IActionResult> AddPassword([FromBody] PasswordVault password)
        {
            try
            {
                _logger.LogInformation($"Gelen veri: SiteName={password?.SiteName}, Username={password?.Username}, Password={password?.Password != null}");

                var userId = HttpContext.Session.GetInt32("UserId");
                if (!userId.HasValue)
                    return Json(new { success = false, message = "Oturum süresi dolmuş! Lütfen tekrar giriş yapın." });

                if (password == null)
                    return Json(new { success = false, message = "Geçersiz veri gönderildi!" });

                if (string.IsNullOrEmpty(password.SiteName) || string.IsNullOrEmpty(password.Username) || string.IsNullOrEmpty(password.Password))
                {
                    _logger.LogWarning($"Eksik veri: SiteName={!string.IsNullOrEmpty(password.SiteName)}, Username={!string.IsNullOrEmpty(password.Username)}, Password={!string.IsNullOrEmpty(password.Password)}");
                    return Json(new { success = false, message = "Lütfen tüm alanları doldurun!" });
                }

                password.UserId = userId.Value;
                var result = await _passwordVaultService.AddPasswordAsync(password);
                
                if (result == null)
                    return Json(new { success = false, message = "Şifre eklenirken bir hata oluştu!" });

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Şifre eklenirken bir hata oluştu");
                return Json(new { success = false, message = "Şifre eklenirken bir hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordVault password)
        {
            try
            {
                _logger.LogInformation($"Güncelleme için gelen veri: Id={password?.Id}, SiteName={password?.SiteName}, Username={password?.Username}, Password={password?.Password != null}");

                var userId = HttpContext.Session.GetInt32("UserId");
                if (!userId.HasValue)
                    return Json(new { success = false, message = "Oturum süresi dolmuş! Lütfen tekrar giriş yapın." });

                if (password == null)
                    return Json(new { success = false, message = "Geçersiz veri gönderildi!" });

                if (password.Id <= 0)
                    return Json(new { success = false, message = "Geçersiz şifre ID'si!" });

                if (string.IsNullOrEmpty(password.SiteName) || string.IsNullOrEmpty(password.Username) || string.IsNullOrEmpty(password.Password))
                {
                    _logger.LogWarning($"Eksik veri: SiteName={!string.IsNullOrEmpty(password.SiteName)}, Username={!string.IsNullOrEmpty(password.Username)}, Password={!string.IsNullOrEmpty(password.Password)}");
                    return Json(new { success = false, message = "Lütfen tüm alanları doldurun!" });
                }

                password.UserId = userId.Value;
                var result = await _passwordVaultService.UpdatePasswordAsync(password);
                
                if (result == null)
                {
                    _logger.LogWarning($"Şifre güncellenemedi. ID={password.Id}, UserId={password.UserId}");
                    return Json(new { success = false, message = "Şifre güncellenemedi! Şifrenin size ait olduğundan emin olun." });
                }

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Şifre güncellenirken bir hata oluştu");
                return Json(new { success = false, message = "Şifre güncellenirken bir hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeletePassword(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
                return Json(new { success = false, message = "Oturum süresi dolmuş!" });

            var result = await _passwordVaultService.DeletePasswordAsync(id, userId.Value);
            return Json(new { success = result });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
