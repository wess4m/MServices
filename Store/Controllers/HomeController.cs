using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Store.Models;
using Store.Service.IService;
using System.Diagnostics;

namespace Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _ProductService;
        public HomeController(IProductService ProductService)
        {
            _ProductService = ProductService;
        }
        public async Task<IActionResult> Index()
        {
            List<ProductDto>? Products = new();
            ResponseDTO? Response = await _ProductService.GetAllProducts();
            if (Response != null && Response.IsSuccess)
            {
                Products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(Response.Result));
            }
            else
            {
                @TempData["Error"] = Response?.Message;
            }
            return View(Products);
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
