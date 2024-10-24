namespace JpsStreet.Services.OrderApi.Models.DTo
{
    public class StripeRequestDTo
    {
        public string? StripeSessionUrl { get; set; }
        public string? StripeSessionId { get; set; }
        public string? ApproveUrl { get; set; }
        public string? CancelUrl { get; set; }
        public OrderHeaderDTo? OrderHeader { get; set; }
    }
}
