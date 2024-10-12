namespace JpsStreet.Web.Models
{
    public class LoginResponseDTo
    {
        public UserDTo? User { get; set; }
        public string? Token { get; set; }
    }
}
