using AutoMapper;
using JpsStreet.Services.OrderApi.Models;
using JpsStreet.Services.OrderApi.Models.DTo;

namespace JpsStreet.Services.OrderApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<OrderHeaderDTo, CartHeaderDTo>()
                    .ForMember(dest => dest.CartTotal, opt => opt.MapFrom(src => src.OrderTotal))
                    .ReverseMap();

                config.CreateMap<CartDetailsDTo, OrderDetailsDTo>()
                    .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                    .ReverseMap();

                config.CreateMap<OrderHeader, OrderHeaderDTo>().ReverseMap();
                config.CreateMap<OrderDetails, OrderDetailsDTo>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
