﻿namespace JpsStreet.Services.ShoppingCartApi.Models.DTo
{
    public class CouponDTo
    {
        public int CouponId { get; set; }
        public string? CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}