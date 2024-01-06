using Bookstore.Data;
using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Bookstore.Controllers{

    public class BorrowingController : Controller{

        public readonly ApplicationDbContext _context;
        public BorrowingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index(int page = 1)
        {
            try{
                if(page<1)page=1;
                var RealPage = page - 1;

                var Borrowings = (_context.Borrowings != null)?_context.Borrowings
                                                .Include(b=>b.Book)
                                                    .Include(b=>b.Client)
                                                        .OrderBy(b=>b.Id)
                                                            .Skip((RealPage*5))
                                                                .Take(5)
                                                                 .ToList():new List<Borrowing>();
                setPaginationConfig(page,5.0);
                return View(Borrowings);
            }catch(Exception)
            {
                return View();
            }
        }

        [Authorize]
        public IActionResult SelectBook()
        {
            try{
                var Books = (_context.Books!=null)?_context.Books.Include(b=>b.Category).Include(b=>b.Borrowings).ToList():new List<Book>();
                return View(Books);
            }catch(Exception){
                return View();
            }
        }

        [Authorize]
        public IActionResult BorrowBook(int? id)
        {
            try{
                if(id == null)return NotFound();

                var Book = _context.Books.Find(id);
                if(Book == null) return NotFound();

                var Clients = (_context.Clients!=null)?_context.Clients.ToList():new List<Client>();
                ViewData["Book"] = Book;
                return View("SelectClient",Clients);
            }catch(Exception)
            {
                return NotFound();
            }
        }

        [Authorize]
        public IActionResult Create(int id, int book)
        {
            try{
                if(id == null || book == null)return NotFound();
                var Book = _context.Books.Find(book);
                if(Book == null) return NotFound();

                var Client = _context.Clients.Find(id);

                if(Client == null) return NotFound();
                ViewData["Book"] = Book;
                ViewData["Client"] = Client;
                return View();
            }catch(Exception)
            {
                TempData["ErrorMessage"] = "Ocorreu um problema tente outra vez";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([Bind("RequestDate","ReturnDate","Returned","PrizeViolated","BookId","ClientId")]Borrowing Borrowing)
        {
            try{
                Console.WriteLine("ID:"+Borrowing.Id);
                Console.WriteLine("Cli_Id:"+Borrowing.ClientId);
                Console.WriteLine("Book_Id:"+Borrowing.BookId);
                Console.WriteLine("Return:"+Borrowing.ReturnDate);
                    _context.Entry(Borrowing).Property("Id").IsModified = false;
                    _context.Borrowings.Add(Borrowing);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Emprestimo Concluido";
                    return RedirectToAction("Index");
                
                
            }catch(Exception ex)
            {
                TempData["ErrorMessage"] = @"Ocorreu um problema tente outra vez {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        [Authorize]
        public IActionResult Show(int? id)
        {
            try{
                if(id == null)
                {
                    return NotFound();
                }

                var Borrowing = _context.Borrowings.Include(b=>b.Client)
                                                        .Include(b=>b.Book)
                                                            .Include(b=>b.Penalty)
                                                                .ToList().Find(b=>b.Id == id);
                if(Borrowing == null){
                    TempData["ErrorMessage"] = "Emprestimo não encontrado";
                    return RedirectToAction("Index");
                }
                return View(Borrowing);
            }catch(Exception e)
            {
                TempData["ErrorMessage"] = "Ocorreu um problema tente outra vez";
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        [Authorize]
        public IActionResult Devolver(int? id,Borrowing updateBorrowing)
        {

            try{
                if(id == null)return NotFound();
                var borrowing = _context.Borrowings.Find(updateBorrowing.Id);

                if (borrowing == null) return NotFound();
                borrowing.Returned = true;
                if(borrowing.ReturnDate < DateTime.Now){
                    borrowing.PrizeViolated = true;
                }
                _context.Borrowings.Update(borrowing);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Livro Devolvido";
                return RedirectToAction("Index");
            }catch(Exception e)
            {
                TempData["ErrorMessage"] = "Ocorreu um problema tente outra vez";
                return RedirectToAction("Index");
            }

        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(int? id)
        {

            try{
                if(id == null)return NotFound();
                var borrowing = _context.Borrowings.Find(id);

                if (borrowing == null) return NotFound();
                _context.Borrowings.Remove(borrowing);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Empréstimo Removido";
                return RedirectToAction("Index");
            }catch(Exception e)
            {
                TempData["ErrorMessage"] = "Ocorreu um problema tente outra vez";
                return RedirectToAction("Index");
            }

        }


        private void setPaginationConfig(int page, double size)
        {
                double pages = _context.Borrowings.Count() / size;
                ViewData["total_pages"] = Math.Ceiling(pages);
                Console.WriteLine("TOTAL PAGES = " +ViewData["total_pages"] );
                ViewData["actual_page"] = page;
                ViewData["next_page"] = page + 1;
                ViewData["prev_page"] = page==1?page:page - 1;
                
        }

    }
}