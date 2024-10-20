namespace JpsStreet.Services.EmailApi.Models.DTo
{
    public class CartDTo
    {
        public CartHeaderDTo? CartHeader { get; set; }
        public IEnumerable<CartDetailsDTo>? CartDetails { get; set; }
    }
}
