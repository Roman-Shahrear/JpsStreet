namespace JpsStreet.Services.OrderApi.Models.DTo
{
    public class CartDTo
    {
        public CartHeaderDTo? CartHeader { get; set; }
        public IEnumerable<CartDetailsDTo>? CartDetails { get; set; }
    }
}
