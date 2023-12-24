
using Bookstore.Data;
using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Controllers{

    public class BookController : Controller{

        private readonly ApplicationDbContext _context;
        
        public BookController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try{
                var Books = (_context.Books!=null) ? _context.Books.Include(b=>b.Category)
                                                                    .Include(b=>b.Borrowings)
                                                                        .ToList() : new List<Book>();

                return View(Books);
            }catch(Exception){
                return View();
            }
        }

        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();
            ViewData["categories"] = categories;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            try{
                if(ModelState.IsValid)
                {
                    _context.Books.Add(book);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Livro Adicionado";
                    return RedirectToAction("Index");
                }else{
                    TempData["ErrorMessage"] = "Não foi possível adicionar  o livro, tente novamente.";
                    return View();
                }
            }catch(Exception)
            {
                TempData["ErrorMessage"] = "Não foi possível adicionar  o livro, tente novamente.";
                return View();
                
            }
        }

        public IActionResult Details(int? id)
        {
            if(id == null)
            return NotFound();

            try{
                var Book = _context.Books.Find(id);
                if(Book != null)
                {
                    return View(Book);
                }
                TempData["ErrorMessage"] = "Livro nao encontrado.";
                return RedirectToAction("Index");
            }catch(Exception)
            {
                TempData["ErrorMessage"] = "Não foi possível adicionar  o livro, tente novamente.";
                return View();
            }
        }
    }
}