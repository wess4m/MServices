using Store.Models;

namespace Store.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDTO?> loginAsync(LoginRequestDTO loginRequestDTO);
        Task<ResponseDTO?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO);
        Task<ResponseDTO?> AssignRoleAsync(RegistrationRequestDTO registrationRequestDTO);
        Task SignInUser(LoginResponseDTO LoginResponseDTO, Microsoft.AspNetCore.Http.HttpContext httpContent);
    }
}
