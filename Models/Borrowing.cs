
namespace Bookstore.Models
{
    public class Borrowing
    {
        public int Id{get;set;}
        
        public DateTime RequestDate{get;set;} = DateTime.Now;
        public DateTime ReturnDate{get;set;}
        public bool Returned{get;set;} = false;
        public bool PrizeViolated{get;set;} = false;
        public int BookId{get;set;}
        public Book Book{get;set;} = null!;
        public int ClientId{get;set;} 
        public Client Client{get;set;}= null!;
        public Penalty? Penalty{get;set;} 
        
    }    
}