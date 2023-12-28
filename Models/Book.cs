
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace Bookstore.Models
{
    public class Book
    {
        public int Id{get;set;}

        [Required(ErrorMessage="O 'titulo' é obrigatório")]
        public string Title{get;set;} = null!;

        [Required(ErrorMessage="O 'Isbn' é obrigatório")]
        public string Isbn{get;set;} = null!;

        [Required(ErrorMessage="O nome do 'Autor' é obrigatório")]
        public string Autor{get;set;} = null!;

        [Required(ErrorMessage="Forneça o ano de publicação")]
        public int Year{get;set;}

        public string? Cover{get;set;}
        
        [NotMapped]
        public IFormFile? CoverFile { get; set; }

        public int Qtd{get;set;}

        [Required(ErrorMessage="Digite o valor de empréstimo")]
        public decimal Value{get;set;}

        public int CategoryId{get;set;}

        public Category? Category{get;set;} = null!;
        public List<Borrowing> Borrowings {get;set;} = new List<Borrowing>();
        
        

    }    
}