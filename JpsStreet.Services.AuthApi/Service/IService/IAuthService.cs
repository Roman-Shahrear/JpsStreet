using JpsStreet.Services.AuthApi.Models.DTo;

namespace JpsStreet.Services.AuthApi.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDTo registrationRequestDTo);
        Task<LoginResponseDTo> Login(LoginRequestDTo loginRequestDTo);
        Task<bool> AssaignRole(string email, string roleName);
    }
}
