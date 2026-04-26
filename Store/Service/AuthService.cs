using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Store.Models;
using Store.Service.IService;
using Store.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Store.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;

        public AuthService(HttpClient httpClient, ITokenProvider tokenProvider)
        {
            _baseService = new BaseService(httpClient, tokenProvider);
        }

        public async Task<ResponseDTO?> AssignRoleAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDTO,
                Url = SD.AuthAPIBase+ "/api/auth/AssignRole" // Relative path
            }, withBearer: false);
        }

        public async Task<ResponseDTO?> loginAsync(LoginRequestDTO loginRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequestDTO,
                Url = SD.AuthAPIBase + "/api/auth/login" // Relative path
            }, withBearer: false);
        }

        public async Task<ResponseDTO?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDTO,
                Url = SD.AuthAPIBase + "/api/auth/register" // Relative path
            }, withBearer: false);
        }

        public async Task SignInUser(LoginResponseDTO model, HttpContext _HttpContext)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await _HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
