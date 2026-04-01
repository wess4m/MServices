using NuGet.Common;
using Store.Service.IService;
using Store.Utility;

namespace Store.Service
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void ClearToken()
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(SD.TokenCookieKey);
        }

        public string? GetToken()
        {
            string? token = null;
            _httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(SD.TokenCookieKey, out token);
            return token;
        }

        public void SetToken(string token)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(SD.TokenCookieKey, token);
        }
    }
}
