using JpsStreet.Services.AuthApi.Data;
using JpsStreet.Services.AuthApi.Models;
using JpsStreet.Services.AuthApi.Models.DTo;
using JpsStreet.Services.AuthApi.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace JpsStreet.Services.AuthApi.Service
{
    public class AuthService : IAuthService
    {
        private readonly AuthAppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(AuthAppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Task<LoginResponseDTo> Login(LoginRequestDTo loginRequestDTo)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Register(RegistrationRequestDTo registrationRequestDTo)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDTo.Email.ToLower(),
                Email = registrationRequestDTo.Email,
                NormalizedEmail = registrationRequestDTo.Email.ToLower(),
                Name = registrationRequestDTo.Name,
                PhoneNumber = registrationRequestDTo.PhoneNumber,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDTo.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDTo.Email);

                    UserDTo userDto = new()
                    {
                        Email = userToReturn.Email,
                        ID = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    return "";

                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex) 
            {
                
            }
            return "Error Encounter";
        }
    }
}
