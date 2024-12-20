﻿namespace JpsStreet.Services.OrderApi.Models.DTo
{
    public class OrderDetailsDTo
    {
        public int OrderDetailsId { get; set; }
        public int OrderHeaderId { get; set; }
        public int ProductId { get; set; }
        public ProductDTo? Product { get; set; }
        public int Count { get; set; }
        public string? ProductName { get; set; }
        public double Price { get; set; }
    }
}
