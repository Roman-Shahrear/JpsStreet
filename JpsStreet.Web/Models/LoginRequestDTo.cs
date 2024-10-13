using System.ComponentModel.DataAnnotations;

namespace JpsStreet.Web.Models
{
    public class LoginRequestDTo
    {
       [Required] public string? UserName { get; set; }
        [Required]public string? Password { get; set; }
    }
}
