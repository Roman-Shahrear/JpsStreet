using System.ComponentModel.DataAnnotations;

namespace JpsStreet.Services.OrderApi.Models
{
    public class OrderHeader
    {
        [Key]public int OrderHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        public double OrderTotal { get; set; }
    }
}
