using Microsoft.AspNetCore.Mvc;
using SalePoint.Primitives.Interfaces;
using SalePoint.Primitives;

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
            try
            {
                TokenAuth? tokenAuth = await _userRepository.Login(access);

                if (tokenAuth != null && !string.IsNullOrEmpty(tokenAuth.Token)) //&& tokenAuth.UserName != null)
                {
                    HttpContext.Session.SetString("TokenAuth", tokenAuth.Token);
                    return Json(true);
                }
                return Json(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> LogOut()
        {
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return Redirect("~/Login/Index");
        }
    }
}