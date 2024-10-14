using System.ComponentModel.DataAnnotations;

namespace JpsStreet.Services.ProductApi.Models.DTo
{
    public class ProductDTo
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
        public string? ImageUrl { get; set; }
    }
}
