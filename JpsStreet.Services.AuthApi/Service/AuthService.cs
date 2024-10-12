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
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(AuthAppDbContext db,
                UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager,
                IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssaignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if(user != null)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    // create role if doeen't exist
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTo> Login(LoginRequestDTo loginRequestDTo)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTo.UserName.ToLower());
            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDTo.Password);
            if(user == null || isValid == false)
            {
                return new LoginResponseDTo() { User = null, Token = "" };
            }

            // If user is found, Generate Jwt Token
            var token = _jwtTokenGenerator.GenerateToken(user);

            UserDTo userDTo = new()
            {
                Email = user.Email,
                ID = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
            };

            LoginResponseDTo loginResponseDTo = new()
            {
                User = userDTo,
                Token = token
            };
            return loginResponseDTo;
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
