using JpsStreet.Web.Models;
using JpsStreet.Web.Service.IService;
using JpsStreet.Web.Utility;

namespace JpsStreet.Web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTo?> CreateCouponsAsync(CouponDTo couponDto)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.POST,
                Data = couponDto,
                Url = SD.CouponApiBase + "/api/coupon"
            });
        }

        public async Task<ResponseDTo?> DeleteCouponsAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.CouponApiBase + "/api/coupon/" + id
            });
        }

        public async Task<ResponseDTo?> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponApiBase + "/api/coupon"
            });
        }

        public async Task<ResponseDTo?> GetCouponAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponApiBase + "/api/coupon/GetByCode/" + couponCode
            });
        }

        public async Task<ResponseDTo?> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponApiBase + "/api/coupon/" + id
            });
        }

        public async Task<ResponseDTo?> UpdateCouponsAsync(CouponDTo couponDto)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.PUT,
                Data = couponDto,
                Url = SD.CouponApiBase + "/api/coupon"
            });
        }
    }
}