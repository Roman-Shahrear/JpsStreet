using JpsStreet.Web.Models;
using JpsStreet.Web.Service.IService;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static JpsStreet.Web.Utility.SD;

namespace JpsStreet.Web.Service
{
    public class BaseService : IBaseService
    {
        // For API call use IHttpClientFactory
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        public async Task<ResponseDTo?> SendAsync(RequestDTo requestDto)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("JpsStreetApi");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");

                // token

                message.RequestUri = new Uri(requestDto.Url);
                // For serialization like it might to be put or delete then we have to do this
                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage? apiResponse = null;

                message.Method = requestDto.ApiType switch
                {
                    // switch case to convert in expression
                    ApiType.POST => HttpMethod.Post,
                    ApiType.DELETE => HttpMethod.Delete,
                    ApiType.PUT => HttpMethod.Put,
                    _ => HttpMethod.Get,
                };

                apiResponse = await client.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new ResponseDTo { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Forbidden:
                        return new ResponseDTo { IsSuccess = false, Message = "Access Denied" };
                    case HttpStatusCode.Unauthorized:
                        return new ResponseDTo { IsSuccess = false, Message = "Unauthorized" };
                    case HttpStatusCode.InternalServerError:
                        return new ResponseDTo { IsSuccess = false, Message = "Internal Server Error" };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDTo>(apiContent);
                        return apiResponseDto;
                }
            }
            catch (Exception ex)
            {
                var dto = new ResponseDTo
                {
                    Message = ex.Message.ToString(),
                    IsSuccess = false
                };
                return dto;
            }
        }
    }
}