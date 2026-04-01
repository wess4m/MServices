using AuthAPI.Models;

namespace AuthAPI.Service.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(UserDTO userDTO, IEnumerable<string> roles);
    }
}
