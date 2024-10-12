using JpsStreet.Services.AuthApi.Models;

namespace JpsStreet.Services.AuthApi.Service.IService
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser);
    }
}
