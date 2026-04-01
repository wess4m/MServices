using Newtonsoft.Json;
using Store.Models;
using Store.Service.IService;
using System.Diagnostics;
using System.Text;
using static Store.Utility.SD;

namespace Store.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;
        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }
        public async Task<ResponseDTO?> SendAsync(RequestDTO RequestDTO, bool withBearer = true)
        {
            HttpClient client = _httpClientFactory.CreateClient("StoreAPI");
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");
            //Authintication token goes here
            if (withBearer)
            {
                var token = _tokenProvider.GetToken();
                if (!string.IsNullOrEmpty(token))
                {
                    message.Headers.Add("Authorization", $"Bearer {token}");
                }
            }
            message.RequestUri = new Uri(RequestDTO.Url);
            if (RequestDTO.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(RequestDTO.Data), Encoding.UTF8, "application/json");
            }
            switch (RequestDTO.ApiType)
            {
                case ApiType.GET:
                    message.Method = HttpMethod.Get;
                    break;
                case ApiType.POST:
                    message .Method = HttpMethod.Post;
                    break;
                case ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;
            }
            HttpResponseMessage? response = await client.SendAsync(message);
            try
            {
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.NotFound:
                    return new() { IsSuccess = false, Message="Not Found"};
                case System.Net.HttpStatusCode.Forbidden:
                    return new() { IsSuccess = false, Message = "Access Denied" };
                case System.Net.HttpStatusCode.Unauthorized:
                    return new() { IsSuccess = false, Message = "Unauthorized" };
                case System.Net.HttpStatusCode.InternalServerError:
                    return new() { IsSuccess = false, Message = "Internal Server Error" };
                default:
                    var ApiContent= await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ResponseDTO>(ApiContent);  
            }
            }
            catch (Exception ex)
            {
                return new ResponseDTO() { IsSuccess = false, Message = ex.Message };
            }

        }
    }
}
