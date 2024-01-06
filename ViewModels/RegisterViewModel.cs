
using System.ComponentModel.DataAnnotations;

namespace Bookstore.ViewModels{

    public class RegisterViewModel{


        [Required]
        [EmailAddress]
        public string Email{get;set;}

        [Required]
        public string UserName{get;set;}

        [Required]
        [StringLength(100, ErrorMessage="The {0} must be  at least {2} characters long",MinimumLength =6)]
        [DataType(DataType.Password)]
        public string Password{get;set;}

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="The password and confirmation password do not match")]
        public string ConfirmPassword{get;set;}

        public string? ReturnUrl{get;set;}

    }
}