﻿using System.ComponentModel.DataAnnotations;

namespace JpsStreet.Web.Models
{
    public class CartHeaderDTo
    {
        public int CartHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        public double CartTotal { get; set; }
        [Required] public string? Name { get; set; }
        [Required] public string? Phone { get; set; }
        [Required][EmailAddress(ErrorMessage = "Invalid email format.")] public string? Email { get; set; }

    }
}

