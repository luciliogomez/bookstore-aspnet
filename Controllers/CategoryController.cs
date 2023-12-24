
using Bookstore.Data;
using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers{

    public class CategoryController : Controller{

        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try{
                var Categories = _context.Categories!=null?_context.Categories.ToList():new List<Category>();
                
                return View(Categories);
            }catch(Exception)
            {
                return View();
            }
            
        }

        public  IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category Category)
        {
            try{
                if(ModelState.IsValid)
                {
                    _context.Categories.Add(Category);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Categoria Adicionada";
                    return RedirectToAction("Index");
                }
                return View();
            }catch(Exception)
            {
                TempData["ErrorMessage"] = "Erro ao adicionar categoria";
                return View();
            }
        }

        public IActionResult Edit(int? id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(int? id,Category Category)
        {
            return View();
        }

        public IActionResult Delete(int? id)
        {
            return View();
        }
    
    }
}