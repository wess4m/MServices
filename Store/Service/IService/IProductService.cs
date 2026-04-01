using Store.Models;

namespace Store.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDTO?> GetProductAsync(string ProducteCode);
        Task<ResponseDTO?> GetAllProducts();
        Task<ResponseDTO?> GetProductByIdAsync(int id);
        Task<ResponseDTO?> CreateProductAsync(ProductDto ProductDto);
        Task<ResponseDTO?> UpdateProductAsync(ProductDto ProductDto);
        Task<ResponseDTO?> DeleteProductAsync(int id);
    }
}
