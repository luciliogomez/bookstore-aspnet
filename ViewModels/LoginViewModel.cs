
using System.ComponentModel.DataAnnotations;

namespace Bookstore.ViewModels{

    public class LoginViewModel{


        [Required]
        public string Email{get;set;} = null!;

        public string? UserName{get;set;}

        [Required]
        public string Password{get;set;} = null!;

        public string? ReturnUrl{get;set;}
        

    }
}