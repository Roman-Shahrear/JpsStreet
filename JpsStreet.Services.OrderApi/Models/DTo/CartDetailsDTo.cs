namespace JpsStreet.Services.OrderApi.Models.DTo
{
    public class CartDetailsDTo
    {
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        public CartHeaderDTo? CartHeader { get; set; }
        public int ProductId { get; set; }
        public ProductDTo? Product { get; set; }
        public int Count { get; set; }
    }
}
