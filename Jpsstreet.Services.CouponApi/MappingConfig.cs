using AutoMapper;
using Jpsstreet.Services.CouponApi.Models;
using Jpsstreet.Services.CouponApi.Models.DTo;

namespace Jpsstreet.Services.CouponApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDTo, Coupon>();
                config.CreateMap<Coupon, CouponDTo>();
            });

            return mappingConfig;
        }
    }
}
