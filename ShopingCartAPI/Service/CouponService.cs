using Newtonsoft.Json;
using ShoppingCartAPI.Models.Dto;
using ShoppingCartAPI.Service.IService;

namespace ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<CouponDto> GetCouponByCode(string code)
        {
            var client =_httpClientFactory.CreateClient("Coupon");
            var apiResponse = await client.GetAsync($"/api/coupon/GetByCode/{code}");
            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
            if (response!= null && response.IsSuccess && response.Result != null)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
            }
            return null;
        }
    }
}
