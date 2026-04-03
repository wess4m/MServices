using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Store.Models;
using Store.Service.IService;

namespace Store.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _ProductService;
        public ProductController(IProductService ProductService)
        {
            _ProductService = ProductService;
        }
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto>? Products = new();
            ResponseDTO? Response = await _ProductService.GetAllProducts();
            if (Response != null && Response.IsSuccess)
            {
                Products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(Response.Result));
            }
            else
            {
                TempData["Error"] = Response?.Message;
                return RedirectToAction("Index", "Home");
            }
            return View(Products);
        }
        public async Task<IActionResult> CreateProduct()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDto _ProductDto)
        {
            var response = await _ProductService.CreateProductAsync(_ProductDto);
            if (!response.IsSuccess)
            {
                TempData["Error"] = response?.Message;
            }
            return RedirectToAction(nameof(ProductIndex));
        }
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = await _ProductService.DeleteProductAsync(id);
            if (!response.IsSuccess)
            {
                TempData["Error"] = response?.Message;
            }
            return RedirectToAction(nameof(ProductIndex));
        }
    }
}
