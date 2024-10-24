using JpsStreet.Web.Models;

namespace JpsStreet.Web.Service.IService
{
    public interface IOrderService
    {
        Task<ResponseDTo?> CreateOrder(CartDTo cartDto);
        Task<ResponseDTo?> CreateStripeSession(StripeRequestDTo stripeRequestDto);
        Task<ResponseDTo?> ValidateStripeSession(int orderHeaderId);
        Task<ResponseDTo?> GetAllOrder(string? userId);
        Task<ResponseDTo?> GetOrder(int orderId);
        Task<ResponseDTo?> UpdateOrderStatus(int orderId, string newStatus);
    }
}
