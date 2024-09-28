using System.ComponentModel.DataAnnotations;

namespace Core.Security.Jwt.Identity.Model
{
    public class LoginUser
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
    }
}