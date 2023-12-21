
namespace Bookstore.Models
{
    public class Penalty
    {
        public int Id{get;set;}
        public DateTime DateAplied{get;set;} = DateTime.Now;
        public bool Solved{get;set;} = false;
        public decimal Value{get;set;} = 0;
        public int BorrowingId{get;set;}
        public Borrowing Borrowing{get;set;} = null!;
    }    
}