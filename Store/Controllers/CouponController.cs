using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Store.Models;
using Store.Service.IService;

namespace Store.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
                _couponService = couponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? coupons = new();
            ResponseDTO? Response = await _couponService.GetAllCoupons();
            if (Response != null && Response.IsSuccess)
            {
                coupons = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(Response.Result));
            }
            else
            {
                @TempData["Error"] = Response?.Message;
            }
            return View(coupons);
        }
        public async Task<IActionResult> CreateCoupon()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CouponDto _couponDto)
        {
            await _couponService.CreateCouponAsync(_couponDto);
            return RedirectToAction(nameof(CouponIndex));
        }
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            await _couponService.DeleteCouponAsync(id);
            return RedirectToAction(nameof(CouponIndex));
        }
    }
}
