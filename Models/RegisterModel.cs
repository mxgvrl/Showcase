using System.ComponentModel.DataAnnotations;

namespace AppleShowcase.Data.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage ="Nickname is required")]
        public string Name { get; set; }
     
        [Required(ErrorMessage = "Password  is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
     
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Invalid password")]
        public string ConfirmPassword { get; set; }
    }
}