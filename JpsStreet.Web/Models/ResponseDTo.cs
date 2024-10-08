namespace JpsStreet.Web.Models
{
    public class ResponseDTo
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
    }
}
