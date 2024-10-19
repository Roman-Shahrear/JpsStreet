using JpsStreet.Services.ShoppingCartApi.Models.DTo;
using JpsStreet.Services.ShoppingCartApi.Service.IService;
using Newtonsoft.Json;

namespace JpsStreet.Services.ShoppingCartApi.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<CouponDTo> GetCoupon(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"/api/coupon/getByCode/{couponCode}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resProduct = JsonConvert.DeserializeObject<ResponseDTo>(apiContent);
            if (resProduct != null && resProduct.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDTo>(Convert.ToString(resProduct.Result));
            }
            return new CouponDTo();
        }
    }
}
