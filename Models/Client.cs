
using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class Client
    {
        public int Id{get;set;}
        [Required(ErrorMessage="O nome do 'Cliente' é obrigatório")]
        public string Name{get;set;} = null!;
        [Required(ErrorMessage="O email do 'Cliente' é obrigatório")]
        public string Email{get;set;} = null!;
        [Required(ErrorMessage="Uma Data de nascimento é obrigatória")]
        public DateTime BornDate{get;set;} 
        public bool Active{get;set;} = true;
        public List<Borrowing> Borrowings = new List<Borrowing>();
    }    
}