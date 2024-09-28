using System.ComponentModel.DataAnnotations;

namespace Front.MVC.Models
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "{0} required")]
        [EmailAddress(ErrorMessage = "{0} invalid. ")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "{0} required")]
        public string? Password { get; set; }
    }
}
