using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalePoint.Primitives;
using SalePoint.Primitives.Interfaces;

namespace SalePoint.App.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userRepository.GetAllUsers(HttpContext.Session.GetString("TokenAuth")!));
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> GetAllRols()
        {
            return Ok(await _userRepository.GetRols(HttpContext.Session.GetString("TokenAuth")!));
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet (Name = "{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            return Ok(await _userRepository.GetUserById(userId, HttpContext.Session.GetString("TokenAuth")!));
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]StoreUser storeUser)
        {
            return Ok(await _userRepository.CreateUser(storeUser, HttpContext.Session.GetString("TokenAuth")!));
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody]StoreUser storeUser)
        {
            return Ok(await _userRepository.UpdateUser(storeUser, HttpContext.Session.GetString("TokenAuth")!));
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete(Name = "{userId}")]
        public async Task<IActionResult> DeleteUserById(int userId)
        {
            return Ok(await _userRepository.DeleteUserById(userId, HttpContext.Session.GetString("TokenAuth")!));
        }
    }
}
