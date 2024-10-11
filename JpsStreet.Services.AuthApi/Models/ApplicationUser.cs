using Microsoft.AspNetCore.Identity;

namespace JpsStreet.Services.AuthApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
