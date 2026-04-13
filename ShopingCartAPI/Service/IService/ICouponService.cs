using ShoppingCartAPI.Models.Dto;

namespace ShoppingCartAPI.Service.IService
{
    public interface ICouponService
    {
        public Task<CouponDto> GetCouponByCode(string Code);
    }
}
