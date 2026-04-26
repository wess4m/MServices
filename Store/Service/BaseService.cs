using Newtonsoft.Json;
using Store.Models;
using Store.Service.IService;
using System.Net;
using System.Text;
using static Store.Utility.SD;

namespace Store.Service
{
    public class BaseService : IBaseService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenProvider _tokenProvider;

        public BaseService(HttpClient httpClient, ITokenProvider tokenProvider)
        {
            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
        }

        public async Task<ResponseDTO?> SendAsync(RequestDTO RequestDTO, bool withBearer = true)
        {
            try
            {
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");

                // Authentication
                if (withBearer)
                {
                    var token = _tokenProvider.GetToken();
                    if (!string.IsNullOrEmpty(token))
                    {
                        message.Headers.Add("Authorization", $"Bearer {token}");
                    }
                }

                message.RequestUri = new Uri(RequestDTO.Url, UriKind.RelativeOrAbsolute);

                if (RequestDTO.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(RequestDTO.Data), Encoding.UTF8, "application/json");
                }

                message.Method = RequestDTO.ApiType switch
                {
                    ApiType.POST => HttpMethod.Post,
                    ApiType.PUT => HttpMethod.Put,
                    ApiType.DELETE => HttpMethod.Delete,
                    _ => HttpMethod.Get
                };

                // Use the injected _httpClient
                HttpResponseMessage response = await _httpClient.SendAsync(message);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Access Denied" };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Unauthorized" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal Server Error" };
                    default:
                        var apiContent = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO() { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
