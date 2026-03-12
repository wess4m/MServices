using AuthAPI.Models;

namespace AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegistrationRequestDTO registrationRequestDTO);
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO);
    }
}
