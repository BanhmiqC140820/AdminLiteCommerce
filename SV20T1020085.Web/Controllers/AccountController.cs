using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020085.BusinessLayers;


namespace SV20T1020085.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login() 
        { 
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username="", string password="")
        {
            ViewBag.Username = username;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) {
                ModelState.AddModelError("Error", "Nhập tên và mật khẩu");
                return View();
            }
            var userAcount =UserAccountService.Authorize(username, password);
            if(userAcount == null)
            {
                ModelState.AddModelError("Error", "Đăng nhập thất bại");
                return View();
            }
            // Đăng nhập thành công tạo dữ liệu để lưu thông tin đăng nhập
            var userData = new WebUserData()
            {
                UserId =userAcount.UserID,
                UserName =userAcount.UserName,
                DisplayName = userAcount.FullName,
                Email = userAcount.Email,
                Photo = userAcount.Photo,
                ClientIP = HttpContext.Connection.RemoteIpAddress?.ToString(),
                SessionId = HttpContext.Session.Id,
                AdditionalData ="",
                Roles = userAcount.RoleNames.Split(',').ToList()
            };
            //Thiết lập phiên đăng nhập cho tài khoản
            await HttpContext.SignInAsync(userData.CreatePrincipal());

            return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        public IActionResult AccessDenined()
        {
            return View("AccessDenined");
        }
    }
}
