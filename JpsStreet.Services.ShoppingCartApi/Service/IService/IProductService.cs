using JpsStreet.Services.ShoppingCartApi.Models.DTo;

namespace JpsStreet.Services.ShoppingCartApi.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTo>> GetProducts();
    }
}
