using Store.Models;
using Store.Models.Dto;
using Store.Service.IService;
using Store.Utility;

namespace Store.Service
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IBaseService _baseService;
        public ShoppingCartService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTO> ApplyCouponAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                ApiType = Utility.SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/ApplyCoupon"
            });
        }
        public async Task<ResponseDTO> RemoveCouponAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                ApiType = Utility.SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/RemoveCoupon"
            });
        }

        public async Task<ResponseDTO> CartUpsertAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                ApiType = Utility.SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/CartUpsert"
            });
        }

        public async Task<ResponseDTO> GetCartAsync(string UserID)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                ApiType = Utility.SD.ApiType.GET,
                Url = SD.ShoppingCartAPIBase + $"/api/cart/GetCart/{UserID}"
            });
        }

        public async Task<ResponseDTO> RemoveCartItemAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                ApiType = Utility.SD.ApiType.POST,
                Url = SD.ShoppingCartAPIBase + $"/api/cart/RemoveCartItem/{cartDetailsId}"
            });
        }
    }
}
