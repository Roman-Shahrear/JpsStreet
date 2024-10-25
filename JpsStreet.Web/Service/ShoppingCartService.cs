using JpsStreet.Web.Models;
using JpsStreet.Web.Service.IService;
using JpsStreet.Web.Utility;

namespace JpsStreet.Web.Service
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IBaseService _baseService;

        public ShoppingCartService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTo?> ApplyCouponAsync(CartDTo cartDTo)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDTo,
                Url = SD.ShoppingCartApiBase + "/api/cart/ApplyCoupon"
            });
        }

        public async Task<ResponseDTo?> EmailCart(CartDTo cartDTo)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDTo,
                Url = SD.ShoppingCartApiBase + "/api/cart/EmailCartRequest"
            });
        }

        public async Task<ResponseDTo?> GetShoppingCartByUserIDAsync(string userId)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ShoppingCartApiBase + "/api/cart/GetCart/" + userId
            });
        }

        public async Task<ResponseDTo?> RemoveFromShoppingCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDetailsId,
                Url = SD.ShoppingCartApiBase + "/api/cart/RemoveCart"
            });
        }

        public async Task<ResponseDTo?> UpsertShoppingCartAsync(CartDTo cartDTo)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDTo,
                Url = SD.ShoppingCartApiBase + "/api/cart/CartUpsert"
            });
        }
    }
}