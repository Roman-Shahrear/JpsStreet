using JpsStreet.Web.Models;
using JpsStreet.Web.Service.IService;
using JpsStreet.Web.Utility;

namespace JpsStreet.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTo?> CreateProductsAsync(ProductDTo productDto)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.POST,
                Data = productDto,
                Url = SD.ProductApiBase + "/api/product"
            });
        }

        public async Task<ResponseDTo?> DeleteProductsAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.ProductApiBase + "/api/product/" + id
            });
        }

        public async Task<ResponseDTo?> GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductApiBase + "/api/product"
            });
        }

        public async Task<ResponseDTo?> GetProductAsync(string productCode)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductApiBase + "/api/product/GetByCode/" + productCode
            });
        }

        public async Task<ResponseDTo?> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductApiBase + "/api/product/" + id
            });
        }

        public async Task<ResponseDTo?> UpdateProductsAsync(ProductDTo productDto)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.PUT,
                Data = productDto,
                Url = SD.ProductApiBase + "/api/product"
            });
        }
    }
}