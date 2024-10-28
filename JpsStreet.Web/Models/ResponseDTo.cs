namespace JpsStreet.Web.Models
{
    public class ResponseDTo
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";

        public string? StatusCode { get; set; } // Add this line
        public string? ErrorMessage { get; set; } // Add this line
    }
}
