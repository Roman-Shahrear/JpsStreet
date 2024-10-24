﻿using JpsStreet.Web.Models;
using JpsStreet.Web.Service.IService;
using JpsStreet.Web.Utility;

namespace JpsStreet.Web.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService _baseService;

        public OrderService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDTo?> CreateOrder(CartDTo cartDto)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.OrderApiBase + "/api/order/orderCreate"
            });
        }

        public async Task<ResponseDTo?> CreateStripeSession(StripeRequestDTo stripeRequestDto)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.POST,
                Data = stripeRequestDto,
                Url = SD.OrderApiBase + "/api/order/createStripeSession"
            });
        }

        public async Task<ResponseDTo?> GetAllOrder(string? userId)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.OrderApiBase + "/api/order/getOrders?userId=" + userId
            });
        }

        public async Task<ResponseDTo?> GetOrder(int orderId)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.OrderApiBase + "/api/order/getOrderById/" + orderId
            });
        }

        public async Task<ResponseDTo?> UpdateOrderStatus(int orderId, string newStatus)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.POST,
                Data = newStatus,
                Url = SD.OrderApiBase + "/api/order/updateOrderStatus/" + orderId
            });
        }

        public async Task<ResponseDTo?> ValidateStripeSession(int orderHeaderId)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.POST,
                Data = orderHeaderId,
                Url = SD.OrderApiBase + "/api/order/validateStripeSession"
            });
        }
    }
}
