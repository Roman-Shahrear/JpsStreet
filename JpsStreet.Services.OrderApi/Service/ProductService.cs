using JpsStreet.Services.OrderApi.Models.DTo;
using JpsStreet.Services.ShoppingCartApi.Service.IService;
using Newtonsoft.Json;


namespace JpsStreet.Services.ShoppingCartApi.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<ProductDTo>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/product");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resProduct = JsonConvert.DeserializeObject<ResponseDTo>(apiContent);
            if (resProduct.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDTo>>(Convert.ToString(resProduct.Result));
            }
            return new List<ProductDTo>();
        }
    }
}
