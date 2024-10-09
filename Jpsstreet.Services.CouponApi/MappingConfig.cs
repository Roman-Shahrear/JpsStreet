using AutoMapper;
using JpsStreet.Services.CouponApi.Models;
using JpsStreet.Services.CouponApi.Models.DTo;

namespace JpsStreet.Services.CouponApi
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
