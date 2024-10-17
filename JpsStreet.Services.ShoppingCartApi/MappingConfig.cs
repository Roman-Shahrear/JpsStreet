using AutoMapper;
using JpsStreet.Services.ShoppingCartApi.Models;
using JpsStreet.Services.ShoppingCartApi.Models.DTo;


namespace JpsStreet.Services.ShoppingCartApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeaderDTo, CartHeader>().ReverseMap();
                config.CreateMap<CartDetailsDTo, CartDetails>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
