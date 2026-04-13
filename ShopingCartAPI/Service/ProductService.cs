using ShoppingCartAPI.Models.Dto;
using ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace ShoppingCartAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var apiResponse = await client.GetAsync("/api/product/");
            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
            if (response.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(response.Result));
            }
            return new List<ProductDto>();
        }
    }
}
