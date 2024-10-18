using JpsStreet.Services.ShoppingCartApi.Models.DTo;

namespace JpsStreet.Services.ShoppingCartApi.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDTo> GetCoupon(string couponCode);
    }
}
