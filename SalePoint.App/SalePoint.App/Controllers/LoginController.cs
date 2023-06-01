using Microsoft.AspNetCore.Mvc;
using SalePoint.Primitives.Interfaces;
using SalePoint.Primitives;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace SalePoint.App.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepository;

        public LoginController(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Index([FromBody] Models.Access access)
        {
            StoreUser? storeUser = await _userRepository.Login(access);

            if (storeUser != null && storeUser.UserName != null)
            {
                List<Claim> claims = new()
                {
                 new Claim(ClaimTypes.Sid, storeUser.Id.ToString()),
                 new Claim(ClaimTypes.Name, storeUser.Name),
                 new Claim(ClaimTypes.Role, storeUser.Rol.Name)
                };

                ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return Json(true);
            }
            return Json(false);
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
