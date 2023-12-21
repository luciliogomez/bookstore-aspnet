
using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class User
    {
        public int Id{get;set;}
        [Required(ErrorMessage="O nome do 'Utilizador' é obrigatório")]
        public string Name{get;set;} = null!;
       [Required(ErrorMessage="O email do 'Utilizador' é obrigatório")] 
        public string Email{get;set;}=null!;
        public string? UserName{get;set;}
        [Required(ErrorMessage="Uma senha é obrigatória")]
        public string Password{get;set;} = null!;
        public string? UserImage{get;set;}
        public bool Active{get;set;} = true;
    }    
}