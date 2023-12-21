
using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class Category
    {
        public int Id{get;set;}
        [Required(ErrorMessage="O nome da 'Categoria' é obrigatório")]
        public string Name{get;set;} = null!;
        public List<Book> Books{get;set;} = new List<Book>() ;
    }    
}