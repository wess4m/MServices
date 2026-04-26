using Store.Models;
using Store.Service.IService;
using Store.Utility;

namespace Store.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(HttpClient httpClient, ITokenProvider tokenProvider)
        {
            _baseService = new BaseService(httpClient, tokenProvider);
        }

        public async Task<ResponseDTO?> CreateProductAsync(ProductDto ProductDto)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = ProductDto,
                Url = "/api/Product" // Use relative path; BaseAddress is handled by Aspire
            });
        }

        public async Task<ResponseDTO?> DeleteProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.ProductAPIBase + $"/api/Product/{id}"
            });
        }

        public async Task<ResponseDTO?> GetAllProducts()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/Product"
            });
        }

        public async Task<ResponseDTO?> GetProductAsync(string ProducteCode)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + $"/api/Product/GetByCode/{ProducteCode}"
            });
        }

        public async Task<ResponseDTO?> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + $"/api/Product/{id}"
            });
        }

        public async Task<ResponseDTO?> UpdateProductAsync(ProductDto ProductDto)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.PUT,
                Data = ProductDto,
                Url = SD.ProductAPIBase + "/api/Product"
            });
        }
    }
}
