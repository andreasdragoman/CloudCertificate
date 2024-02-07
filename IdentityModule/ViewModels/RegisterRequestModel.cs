using System.ComponentModel.DataAnnotations;

namespace IdentityModule.ViewModels
{
    public class RegisterRequestModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
    }
}
