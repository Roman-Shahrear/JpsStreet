using JpsStreet.Services.OrderApi.Models.DTo;

namespace JpsStreet.Services.ShoppingCartApi.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTo>> GetProducts();
    }
}
