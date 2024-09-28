using System.ComponentModel.DataAnnotations;

namespace Store.Authentication.API.Domain.Requests
{
    public class RegisterUserRequest
    {
        [Required(ErrorMessage = "{0} required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "{0} required")]
        public string? Cpf { get; set; }

        [Required(ErrorMessage = "{0} required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "{0} required")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "do not conference")]
        public string? ConfirmPassword { get; set; }
    }
}
