using MsEShop.Web.Models;
using MsEShop.Web.Service.IService;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace MsEShop.Web.Service
{
    public class BaseService : IBaseService
    {
        private const string MediaType = "application/json";
        private readonly IHttpClientFactory _httpClientFactory;
        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ResponseDto> SendAsync(RequestDto requestDto)
        {
            try
            {
                HttpRequestMessage request = new();
                request.Headers.Add("Accept", MediaType);
                request.RequestUri = new Uri(requestDto.Uri);
                if (requestDto.Data != null)
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, MediaType);
                }

                switch (requestDto.ApiType)
                {
                    case Enums.ApiType.GET:
                        request.Method = HttpMethod.Get;
                        break;
                    case Enums.ApiType.POST:
                        request.Method = HttpMethod.Post;
                        break;
                    case Enums.ApiType.PUT:
                        request.Method = HttpMethod.Put;
                        break;
                    case Enums.ApiType.DELETE:
                        request.Method = HttpMethod.Delete;
                        break;
                }

                HttpClient client = _httpClientFactory.CreateClient("MsEshopAPI");
                HttpResponseMessage apiResponse = await client.SendAsync(request);

                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { Success = false, Message = "Not Found" };

                    case HttpStatusCode.Unauthorized:
                        return new() { Success = false, Message = "Unauthorized" };

                    case HttpStatusCode.Forbidden:
                        return new() { Success = false, Message = "Forbidden" };

                    case HttpStatusCode.InternalServerError:
                        return new() { Success = false, Message = "Internal Server Error" };

                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;
                }
            }
            catch (Exception e)
            {
                return new ResponseDto()
                {
                    Success = false,
                    Message = "ERROR: " + e.Message
                };
            }
        }
    }
}
