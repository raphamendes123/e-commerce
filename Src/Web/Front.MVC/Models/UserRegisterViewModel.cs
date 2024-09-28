using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Front.MVC.Models
{
    public class UserRegisterViewModel
    {

        [Required(ErrorMessage = "{0} is required")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = " {0} is required")]
        [DisplayName("CPF")]
        [Cpf]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [EmailAddress(ErrorMessage = "{0} invalid. ")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "{0} required")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "do not conference")]
        public string? ConfirmPassword { get; set; }
    }
}
