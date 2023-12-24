using Bookstore.Data;
using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers{

    public class ClientController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try{
                var Clients = (_context.Clients!=null)?_context.Clients.ToList():new List<Client>();
                return View(Clients);
            }catch(Exception)
            {
                return RedirectToAction("Create");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
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
    }
}