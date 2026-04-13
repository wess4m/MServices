using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Store.Models.Dto;
using Store.Service.IService;
using System.IdentityModel.Tokens.Jwt;

namespace Store.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _ShoppingCartService;
        public ShoppingCartController(IShoppingCartService ShoppingCartService)
        {
            _ShoppingCartService = ShoppingCartService;
        }
        public async Task<IActionResult> CartIndex()
        {
            var response = await _ShoppingCartService.GetCartAsync(User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault().Value);
            if (response.IsSuccess && response.Result != null)
            {
                var _cart = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
                return View(_cart);
            }
            return View(new CartDto());
        }
        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartdto)
        {
            if (cartdto != null && cartdto.CartHeaderDto != null && !string.IsNullOrEmpty(cartdto.CartHeaderDto.CouponCode))
            {
                var response = await _ShoppingCartService.ApplyCouponAsync(cartdto);
                if (response.IsSuccess && response.Result != null)
                {
                    TempData["Success"] = "Coupon has been applied successfully";
                }
                else
                {
                    TempData["Error"] = response.Message;
                }
            }
            return RedirectToAction("CartIndex");
        }
        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartdto)
        {
            if (cartdto != null && cartdto.CartHeaderDto != null)
            {
                var response = await _ShoppingCartService.RemoveCouponAsync(cartdto);
                if (response.IsSuccess)
                {
                    TempData["Success"] = "Coupon has been removed successfully";
                }
                else
                {
                    TempData["Error"] = response.Message;
                }
            }
            return RedirectToAction("CartIndex");
        }
        public async Task<IActionResult> RemoveCartItem(int cartDetailsId)
        {
                var response = await _ShoppingCartService.RemoveCartItemAsync(cartDetailsId);
            if (response.IsSuccess)
            {
                TempData["Success"] = "Product has been removed successfully";
                TempData["TotalProductsCount"] = response.Result != null ? JsonConvert.DeserializeObject<int>(Convert.ToString(response.Result)) : 0;
                TempData.Keep("TotalProductsCount");
            }
            else
            {
                TempData["Error"] = response.Message;
            }
            return RedirectToAction("CartIndex");
        }
    }
}
