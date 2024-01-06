
using System.ComponentModel.DataAnnotations;

namespace Bookstore.ViewModels{

    public class CreateViewModel{


        [Required]
        [EmailAddress]
        public string Email{get;set;}

        [Required]
        public string UserName{get;set;}

        

    }
}