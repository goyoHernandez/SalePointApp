﻿using Microsoft.AspNetCore.Authorization;
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

            return Ok(await _productRepository.CreateProduct(product, HttpContext.Session.GetString("TokenAuth")!));
        }

        [Authorize]
        [HttpGet(Name = "pageNumber/{pageNumber}/pageSize/{pageSize}")]

        public async Task<IActionResult> GetAllProducts(int pageNumber, int pageSize)
        {
            return Ok(await _productRepository.GetAllProducts(pageNumber, pageSize, HttpContext.Session.GetString("TokenAuth")!));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetProductsExpiringSoon()
        {
            return Json(await _productRepository.GetProductsExpiringSoon(HttpContext.Session.GetString("TokenAuth")!));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetProductsNearCompletition()
        {
            return Json(await _productRepository.GetProductsNearCompletition(HttpContext.Session.GetString("TokenAuth")!));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            return Ok(await _departmentRepository.GetAllDepartments(HttpContext.Session.GetString("TokenAuth")!));
        }

        [Authorize]
        [HttpGet(Name = "{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            return Ok(await _productRepository.GetProductById(productId, HttpContext.Session.GetString("TokenAuth")!));
        }

        [Authorize]
        [HttpGet(Name = "{barCode}")]
        public async Task<IActionResult> GetProductByBarCode(string barCode)
        {
            return Ok(await _productRepository.GetProductByBarCode(barCode, HttpContext.Session.GetString("TokenAuth")!));
        }

        [Authorize]
        [HttpGet(Name = "{keyWord}")]
        public async Task<IActionResult> GetProductByNameOrDescription(string keyWord)
        {
            return Ok(await _productRepository.GetProductByNameOrDescription(keyWord, HttpContext.Session.GetString("TokenAuth")!));
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            if (User.Identity is ClaimsIdentity claimsIdentity)
                product.UserId = int.Parse(claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()!);

            return Ok(await _productRepository.UpdateProduct(product, HttpContext.Session.GetString("TokenAuth")!));
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut(Name = "productId/{productId}")]
        public async Task<IActionResult> UpdateStockProduct([FromBody] int stock, int productId)
        {
            return Ok(await _productRepository.UpdateStockProduct(stock, productId, HttpContext.Session.GetString("TokenAuth")!));
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete(Name = "{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            int userId = 0;

            if (User.Identity is ClaimsIdentity claimsIdentity)
                userId = int.Parse(claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).SingleOrDefault()!);

            return Ok(await _productRepository.DeleteProduct(productId, userId, HttpContext.Session.GetString("TokenAuth")!));
        }

        [Authorize]
        [HttpGet(Name = "{keyWord}/pageNumber/{pageNumber}/pageSize/{pageSize}")]
        public async Task<ActionResult> GetProductByNameOrDescriptionPaginate(string keyWord, int pageNumber, int pageSize)
        {
            return Ok(await _productRepository.GetProductByNameOrDescriptionPaginate(keyWord, pageNumber, pageSize, HttpContext.Session.GetString("TokenAuth")!));
        }
    }
}