using Bookstore.Data;
using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Controllers{

    public class PenaltyController : Controller{

        public readonly ApplicationDbContext _context;
        public PenaltyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page = 1)
        {
            try{
                if(page<1)page=1;
                var RealPage = page - 1;

                var Penalties = (_context.Penalties != null)?_context.Penalties
                                                .Include(b=>b.Borrowing)
                                                .ThenInclude(b=>b.Book)
                                                .Include(b=>b.Borrowing)
                                                .ThenInclude(b=>b.Client)
                                                        .OrderBy(c=>c.Id)
                                                        .Skip((RealPage*5))
                                                            .Take(5).ToList():new List<Penalty>();
                setPaginationConfig(page,5.0);

                return View(Penalties);
            }catch(Exception)
            {
                return View();
            }
        }


        public IActionResult Create(int? id)
        {
            try{
                if(id == null)return NotFound();
                var Borrowing = _context.Borrowings.Include(b=>b.Client)
                                                        .Include(b=>b.Book)
                                                            .Include(b=>b.Penalty)
                                                                .ToList().Find(b=>b.Id == id);
                ViewData["Borrowing"] = Borrowing;
                return View();
            }catch(Exception)
            {
                TempData["ErrorMessage"] = "Ocorreu um problema tente outra vez";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Create([Bind("DateAplied","Solved","Value","BorrowingId")]Penalty Penalty)
        {
            try{
                    // _context.Entry(Borrowing).Property("Id").IsModified = false;
                    _context.Penalties.Add(Penalty);
                    
                    // suspend Client
                    int BorrowingId = Penalty.BorrowingId;
                    var Borrowing = _context.Borrowings.Find(BorrowingId);
                    if(Borrowing !=null)
                    {
                        var Client = _context.Clients.Find(Borrowing.ClientId);
                        if(Client!=null)
                        {
                            Client.Active = false;
                            _context.Clients.Update(Client);
                        }
                    }

                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Multa Aplicada";
                    return RedirectToAction("Index");
                
                
            }catch(Exception ex)
            {
                TempData["ErrorMessage"] = "Ocorreu um problema tente outra vez";
                return RedirectToAction("Index");
            }
        }


        public IActionResult Show(int? id)
        {
            try{
                if(id == null)
                {
                    return NotFound();
                }

                var Penalty = (_context.Penalties != null)?_context.Penalties
                                                .Include(b=>b.Borrowing)
                                                .ThenInclude(b=>b.Book)
                                                .Include(b=>b.Borrowing)
                                                .ThenInclude(b=>b.Client)
                                                        .ToList().Find(b=>b.Id == id):null;
                if(Penalty == null){
                    TempData["ErrorMessage"] = "Multa não encontrada";
                    return RedirectToAction("Index");
                }
                return View(Penalty);
            }catch(Exception e)
            {
                TempData["ErrorMessage"] = "Ocorreu um problema tente outra vez";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Pay(int? id)
        {
            try{
                if(id == null)
                {
                    return NotFound();
                }

                var Penalty = (_context.Penalties != null)?_context.Penalties
                                                .Include(b=>b.Borrowing)
                                                .ThenInclude(b=>b.Book)
                                                .Include(b=>b.Borrowing)
                                                .ThenInclude(b=>b.Client)
                                                        .ToList().Find(b=>b.Id == id):null;
                if(Penalty == null){
                    TempData["ErrorMessage"] = "Multa não encontrada";
                    return RedirectToAction("Index");
                }
                return View(Penalty);
            }catch(Exception e)
            {
                TempData["ErrorMessage"] = "Ocorreu um problema tente outra vez";
                return RedirectToAction("Index");
            }
        }

        [HttpPost,ActionName("Pay")]
        public IActionResult PayPenalty(int? id)
        {

            try{
                if(id == null)return NotFound();
                var Penalty = _context.Penalties.Include(p => p.Borrowing).ToList().Find(p => p.Id == id);

                if (Penalty == null) return NotFound();
                Penalty.Solved = true;
                _context.Penalties.Update(Penalty);

                    if(Penalty.Borrowing !=null)
                    {
                        var Client = _context.Clients.Find(Penalty.Borrowing.ClientId);
                        if(Client!=null)
                        {
                            Client.Active = true;
                            _context.Clients.Update(Client);
                        }
                    }

                _context.SaveChanges();
                TempData["SuccessMessage"] = "Pagamento Concluido";
                return RedirectToAction("Index");
            }catch(Exception e)
            {
                TempData["ErrorMessage"] = "Ocorreu um problema tente outra vez";
                return RedirectToAction("Index");
            }

        }

         private void setPaginationConfig(int page, double size)
        {
                double pages = _context.Penalties.Count() / size;
                ViewData["total_pages"] = Math.Ceiling(pages);
                Console.WriteLine("TOTAL PAGES = " +ViewData["total_pages"] );
                ViewData["actual_page"] = page;
                ViewData["next_page"] = page + 1;
                ViewData["prev_page"] = page==1?page:page - 1;
        }
    }
}