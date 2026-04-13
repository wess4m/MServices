using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Store.Models;
using Store.Models.Dto;
using Store.Service.IService;
using Store.ViewModels;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace Store.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _ProductService;
        private readonly IShoppingCartService _ShoppingCartService;
        public HomeController(IProductService ProductService, IShoppingCartService shoppingCartService)
        {
            _ProductService = ProductService;
            _ShoppingCartService = shoppingCartService;
        }
        public async Task<IActionResult> Index()
        {
            HomeIndexViewModel homeIndex = new();
            List<ProductDto>? Products = new();
            ResponseDTO? pResponse = await _ProductService.GetAllProducts();
            if (pResponse != null && pResponse.IsSuccess)
            {
                homeIndex.Products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(pResponse.Result));
            }
            else
            {
                @TempData["Error"] = pResponse?.Message;
            }
            if (User.Identity.IsAuthenticated)
            {
                ResponseDTO? cResponse = await _ShoppingCartService.GetCartAsync(User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value);
                if (cResponse != null && cResponse.IsSuccess)
                {
                    homeIndex.Cart = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(cResponse?.Result));
                    TempData["TotalProductsCount"] = homeIndex.Cart?.CartDetailsDto?.Sum(x => x.Count);
                    TempData.Keep("TotalProductsCount");
                }
                else
                {
                    @TempData["Error"] = cResponse?.Message;
                }
            }
            return View(homeIndex);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Index(string ProductId,string Count, string CartHeaderID)
        {
            try
            {
                var cartHeaderDto = new CartHeaderDto()
                {
                    Id=Convert.ToInt32(CartHeaderID),
                    UserId  = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value
                };
                List<CartDetailsDto> cartDetailsDtoLst = new List<CartDetailsDto>();
                cartDetailsDtoLst.Add(new CartDetailsDto()
                {
                    ProductId = Convert.ToInt32(ProductId),
                    Count = Convert.ToInt32(Count)
                });
                await _ShoppingCartService.CartUpsertAsync(new CartDto()
                {
                    CartHeaderDto = cartHeaderDto,
                    CartDetailsDto = cartDetailsDtoLst
                });
                TempData["Success"] = "Product has been added to the shopping cart.";
            }
            catch(Exception e) {
                TempData["Error"] = e.Message;
            }
            return RedirectToAction("Index");
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
