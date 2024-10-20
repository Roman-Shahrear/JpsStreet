using JpsStreet.Web.Models;

namespace JpsStreet.Web.Service.IService
{
    public interface IShoppingCartService
    {
        Task<ResponseDTo?> GetShoppingCartByUserIDAsync(string userId);
        Task<ResponseDTo?> UpsertShoppingCartAsync(CartDTo cartDTo);
        Task<ResponseDTo?> RemoveFromShoppingCartAsync(int cartDetailsId);
        Task<ResponseDTo?> ApplyCouponAsync(CartDTo cartDTo);
        Task<ResponseDTo?> EmailCart(CartDTo cartDTo);
    }
}
