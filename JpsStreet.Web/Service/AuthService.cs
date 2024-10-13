using JpsStreet.Web.Models;
using JpsStreet.Web.Service.IService;
using JpsStreet.Web.Utility;

namespace JpsStreet.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTo?> AssaignRoleAsync(RegistrationRequestDTo registrationRequestDTo)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDTo,
                Url = SD.AuthApiBase + "/api/auth/assaignRole"
            });
        }

        public async Task<ResponseDTo?> LoginAsync(LoginRequestDTo loginRequestDTo)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequestDTo,
                Url = SD.AuthApiBase + "/api/auth/login"
            },withBearer:false);
        }

        public async Task<ResponseDTo?> RegisterAsync(RegistrationRequestDTo registrationRequestDTo)
        {
            return await _baseService.SendAsync(new RequestDTo()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDTo,
                Url = SD.AuthApiBase + "/api/auth/register"
            },withBearer:false);
        }
    }
}
