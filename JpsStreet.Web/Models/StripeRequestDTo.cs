namespace JpsStreet.Web.Models
{
    public class StripeRequestDTo
    {
        public string? StripeSessionUrl { get; set; }
        public string? StripeSessionId { get; set; }
        public string? ApprovedUrl { get; set; }
        public string? CancelUrl { get; set; }
        public OrderHeaderDTo? OrderHeader { get; set; }
    }
}
