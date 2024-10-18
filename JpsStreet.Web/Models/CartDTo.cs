namespace JpsStreet.Web.Models
{
    public class CartDTo
    {
        public CartHeaderDTo? CartHeader { get; set; }
        public IEnumerable<CartDetailsDTo>? CartDetails { get; set; }
    }
}
