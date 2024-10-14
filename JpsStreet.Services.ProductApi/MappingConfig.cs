using AutoMapper;
using JpsStreet.Services.ProductApi.Models;
using JpsStreet.Services.ProductApi.Models.DTo;

namespace JpsStreet.Services.ProductApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDTo, Product>().ReverseMap();
                //config.CreateMap<Product, ProductDTo>();
            });

            return mappingConfig;
        }
    }
}
