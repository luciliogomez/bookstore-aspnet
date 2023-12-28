
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

        public IActionResult Index(int page = 1)
        {

            try{ 
                if(page<1)page=1;
                var RealPage = page - 1;
                var Categories = _context.Categories!=null?
                                            _context.Categories
                                                    .OrderBy(c=>c.Id)
                                                        .Skip((RealPage*5))
                                                            .Take(5).ToList()
                                                            :new List<Category>();
                setPaginationConfig(page,5.0);
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

         private void setPaginationConfig(int page, double size)
        {
                double pages = _context.Categories.Count() / size;
                ViewData["total_pages"] = Math.Ceiling(pages);
                Console.WriteLine("TOTAL PAGES = " +ViewData["total_pages"] );
                ViewData["actual_page"] = page;
                ViewData["next_page"] = page + 1;
                ViewData["prev_page"] = page==1?page:page - 1;
        }
    
    }
}