using Bookstore.Data;
using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Bookstore.Controllers{

    public class ClientController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index(int page = 1)
        {
            try{
                if(page<1)page=1;
                var RealPage = page - 1;
                var Clients = (_context.Clients!=null)?
                                        _context.Clients
                                            .OrderBy(c=>c.Id)
                                                .Skip((RealPage*5))
                                                    .Take(5).ToList()
                                                        :new List<Client>();
                setPaginationConfig(page,5.0);
                return View(Clients);
            }catch(Exception)
            {
                return RedirectToAction("Create");
            }
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(Client client)
        {
            try{
                if(ModelState.IsValid)
                {
                    _context.Clients.Add(client);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Cliente Adicionado";
                    return RedirectToAction("Index");
                }else
                {
                    return View();
                }
            }catch(Exception)
            {
                TempData["ErrorMessage"] = "Falha ao adicionar Cliente";
                return View();
            }
        }

        [Authorize]
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                TempData["ErrorMessage"] = "Cliente não encontrado";
                return RedirectToAction("Index");
            }
            var Client = _context.Clients.Find(id);
            if(Client == null)
            {
                TempData["ErrorMessage"] = "Cliente não encontrado";
                return RedirectToAction("Index");
            }
            return View(Client);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int? id, Client client)
        {
            try{
                if(client.Id != id)return NotFound();
                if(ModelState.IsValid)
                {
                    _context.Clients.Update(client);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Cliente Alterado";
                    return RedirectToAction("Index");
                }else
                {
                    return View();
                }
            }catch(Exception)
            {
                TempData["ErrorMessage"] = "Falha ao actualizar dados do Cliente";
                return View();
            }
        }

        [Authorize]
        public IActionResult Borrowings(int? id)
        {
            try{
                if(id == null)
            {
                TempData["ErrorMessage"] = "Cliente não encontrado";
                return RedirectToAction("Index");
            }
            var Client = _context.Clients.Include(c=>c.Borrowings).ThenInclude(b=>b.Book).ToList().Find(c=>c.Id == id);
            if(Client == null)
            {
                TempData["ErrorMessage"] = "Cliente não encontrado";
                return RedirectToAction("Index");
            }
            return View(Client);
            }catch(Exception)
            {
                TempData["ErrorMessage"] = "Falha ao buscar dados do Cliente";
                return View();
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(int? id)
        {

            try{
                if(id == null)return NotFound();
                var Client = _context.Clients.Find(id);

                if (Client == null) return NotFound();
                _context.Clients.Remove(Client);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Ciente Removido";
                return RedirectToAction("Index");
            }catch(Exception e)
            {
                TempData["ErrorMessage"] = "Ocorreu um problema tente outra vez";
                return RedirectToAction("Index");
            }

        }
         private void setPaginationConfig(int page, double size)
        {
                double pages = _context.Clients.Count() / size;
                ViewData["total_pages"] = Math.Ceiling(pages);
                Console.WriteLine("TOTAL PAGES = " +ViewData["total_pages"] );
                ViewData["actual_page"] = page;
                ViewData["next_page"] = page + 1;
                ViewData["prev_page"] = page==1?page:page - 1;
        }
    }
}