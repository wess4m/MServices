using Microsoft.AspNetCore.Mvc.Formatters;
using Store.Models;
using Store.Service.IService;
using Store.Utility;

namespace Store.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(HttpClient httpClient, ITokenProvider tokenProvider)
        {
            _baseService = new BaseService(httpClient, tokenProvider);
        }

        public async Task<ResponseDTO?> CreateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = couponDto,
                Url = SD.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDTO?> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.CouponAPIBase + $"/api/coupon/{id.ToString()}"
            });
        }

        public async Task<ResponseDTO?> GetAllCoupons()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDTO?> GetCouponAsync(string couponeCode)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + $"/api/coupon/GetByCode/{couponeCode}"
            });
        }

        public async Task<ResponseDTO?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + $"/api/coupon/{id.ToString()}"
            });
        }

        public async Task<ResponseDTO?> UpdateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.PUT,
                Data = couponDto,
                Url = SD.CouponAPIBase + $"/api/coupon"
            });
        }
    }
}
