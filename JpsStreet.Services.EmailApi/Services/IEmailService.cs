using JpsStreet.Services.EmailApi.Models.DTo;

namespace JpsStreet.Services.EmailApi.Services
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDTo cartDto);
        Task RegisterUserEmailAndLog(string email);
    }
}
