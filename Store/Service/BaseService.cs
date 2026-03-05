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
        public BaseService(IHttpClientFactory httpClientFactory)
        {
                _httpClientFactory = httpClientFactory;
        }
        public async Task<ResponseDto?> SendAsync(RequestDto requestDto)
        {
            HttpClient client = _httpClientFactory.CreateClient("StoreAPI");
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");
            //Authintication token goes here
            message.RequestUri = new Uri(requestDto.URL);
            if (requestDto.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
            }
            switch (requestDto.ApiType)
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
                    return JsonConvert.DeserializeObject<ResponseDto>(ApiContent);  
            }
            }
            catch (Exception ex)
            {
                return new ResponseDto() { IsSuccess = false, Message = ex.Message };
            }

        }
    }
}
