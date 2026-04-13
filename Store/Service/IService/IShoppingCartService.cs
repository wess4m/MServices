using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.Models;
using Store.Models.Dto;
using System.Reflection.PortableExecutable;

namespace Store.Service.IService
{
    public interface IShoppingCartService
    {
        public Task<ResponseDTO> GetCartAsync(string UserID);
        public Task<ResponseDTO> CartUpsertAsync(CartDto cartDTO);
        public Task<ResponseDTO> RemoveCartItemAsync(int cartDetailsId);
        public Task<ResponseDTO> ApplyCouponAsync(CartDto cartDto);
        public Task<ResponseDTO> RemoveCouponAsync(CartDto cartDto);
    }
}
