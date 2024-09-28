using System.ComponentModel.DataAnnotations;

namespace Store.Authentication.API.Domain.Requests
{
    public class LoginUserRequest
    {
        [Required(ErrorMessage = "Campo {0} required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Campo {0} required")]
        public string? Password { get; set; }
    }
}
