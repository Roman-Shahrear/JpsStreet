namespace JpsStreet.Services.AuthApi.Models.DTo
{
    public class LoginResponseDTo
    {
        public UserDTo? User { get; set; }
        public string? Token { get; set; }
    }
}
