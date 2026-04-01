using Store.Models;

namespace Store.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDTO?> SendAsync(RequestDTO RequestDTO, bool withBearer =true);
    }
}
