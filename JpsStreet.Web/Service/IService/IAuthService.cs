using JpsStreet.Web.Models;

namespace JpsStreet.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDTo?> LoginAsync(LoginRequestDTo loginRequestDTo);
        Task<ResponseDTo?> RegisterAsync(RegistrationRequestDTo registrationRequestDTo);
        Task<ResponseDTo?> AssaignRoleAsync(RegistrationRequestDTo registrationRequestDTo);
    }
}
