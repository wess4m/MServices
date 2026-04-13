using ShoppingCartAPI.Models.Dto;

namespace ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetProducts();
    }
}
