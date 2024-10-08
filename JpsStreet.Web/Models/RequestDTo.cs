using static JpsStreet.Web.Utility.SD;

namespace JpsStreet.Web.Models
{
    public class RequestDTo
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string? Url { get; set; }
        public object? Data  { get; set; }
        public string? AccessToken { get; set; }
    }
}
