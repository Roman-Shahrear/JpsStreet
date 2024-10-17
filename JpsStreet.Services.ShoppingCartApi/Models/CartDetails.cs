﻿using JpsStreet.Services.ShoppingCartApi.Models.DTo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JpsStreet.Services.ShoppingCartApi.Models
{
    public class CartDetails
    {
        [Key]public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        [ForeignKey("CartHeaderId")]public CartHeader? CartHeader { get; set; }
        public int ProductId { get; set; }
        [NotMapped]public ProductDTo? Product { get; set; }
        public int Count { get; set; }
    }
}
