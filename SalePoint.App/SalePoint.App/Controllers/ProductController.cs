using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalePoint.Primitives;
using SalePoint.Primitives.Interfaces;
using System.Security.Claims;

namespace SalePoint.App.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public ProductController(IProductRepository productRepository, IDepartmentRepository departmentRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (User.Identity is ClaimsIdentity claimsIdentity)
                product.UserId = int.Parse(claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()!);

            return Ok(await _productRepository.CreateProduct(product));
        }

        [Authorize]
        [HttpGet]

        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _productRepository.GetAllProducts());
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetProductsExpiringSoon()
        {
            return Json(await _productRepository.GetProductsExpiringSoon());
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetProductsNearCompletition()
        {
            return Json(await _productRepository.GetProductsNearCompletition());
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            return Ok(await _departmentRepository.GetAllDepartments());
        }

        [Authorize]
        [HttpGet(Name = "{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            return Ok(await _productRepository.GetProductById(productId));
        }

        [Authorize]
        [HttpGet(Name = "{barCode}")]
        public async Task<IActionResult> GetProductByBarCode(string barCode)
        {
            return Ok(await _productRepository.GetProductByBarCode(barCode));
        }

        [Authorize]
        [HttpGet(Name = "{keyWord}")]
        public async Task<IActionResult> GetProductByNameOrDescription(string keyWord)
        {
            return Ok(await _productRepository.GetProductByNameOrDescription(keyWord));
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            if (User.Identity is ClaimsIdentity claimsIdentity)
                product.UserId = int.Parse(claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()!);

            return Ok(await _productRepository.UpdateProduct(product));
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut(Name = "productId/{productId}")]
        public async Task<IActionResult> UpdateStockProduct([FromBody] int stock, int productId)
        {
            return Ok(await _productRepository.UpdateStockProduct(stock, productId));
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete(Name = "{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            int userId = 0;

            if (User.Identity is ClaimsIdentity claimsIdentity)
                userId = int.Parse(claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()!);

            return Ok(await _productRepository.DeleteProduct(productId, userId));
        }
    }
}