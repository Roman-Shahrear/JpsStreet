using JpsStreet.Web.Models;

namespace JpsStreet.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDTo?> GetCouponAsync(string couponCode);
        Task<ResponseDTo?> GetAllCouponsAsync();
        Task<ResponseDTo?> GetCouponByIdAsync(int id);
        Task<ResponseDTo?> CreateCouponsAsync(CouponDTo couponDto);
        Task<ResponseDTo?> UpdateCouponsAsync(CouponDTo couponDto);
        Task<ResponseDTo?> DeleteCouponsAsync(int id);
    }
}
