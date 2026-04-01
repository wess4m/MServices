using Store.Models;

namespace Store.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDTO?> GetCouponAsync(string couponeCode);
        Task<ResponseDTO?> GetAllCoupons();
        Task<ResponseDTO?> GetCouponByIdAsync(int id);
        Task<ResponseDTO?> CreateCouponAsync(CouponDto couponDto);
        Task<ResponseDTO?> UpdateCouponAsync(CouponDto couponDto);
        Task<ResponseDTO?> DeleteCouponAsync(int id);
    }
}
