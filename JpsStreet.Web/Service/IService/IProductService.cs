using JpsStreet.Web.Models;

namespace JpsStreet.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDTo?> GetProductAsync(string productCode);
        Task<ResponseDTo?> GetAllProductsAsync();
        Task<ResponseDTo?> GetProductByIdAsync(int id);
        Task<ResponseDTo?> CreateProductsAsync(ProductDTo productDto);
        Task<ResponseDTo?> UpdateProductsAsync(ProductDTo productDTo);
        Task<ResponseDTo?> DeleteProductsAsync(int id);
    }
}
