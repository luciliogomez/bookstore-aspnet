
using Bookstore.Data;
using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Bookstore.Controllers{

    public class CategoryController : Controller{

        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
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
        [Authorize]
        public  IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
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

        [Authorize]
        public IActionResult Edit(int? id)
        {
            try{
                if(id==null)
                {
                    TempData["ErrorMessage"] = "Categoria não encontrada";;
                    return RedirectToAction("Index");
                }

                var Category = _context.Categories.Find(id);
                if(Category == null)
                {
                    TempData["ErrorMessage"] = "Categoria não encontrada";
                    return RedirectToAction("Index");
                }
                return View(Category);
            }catch(Exception)
            {
                TempData["ErrorMessage"] = "Ocorreu um erro. Tente novamente";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(int? id,Category Category)
        {
            try{
                if(id == null || Category.Id != id)
                {
                    TempData["ErrorMessage"] = "Ocorreu um erro. Tente novamente";
                    return RedirectToAction("Index");
                }
                if(ModelState.IsValid)
                {
                    _context.Categories.Update(Category);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Categoria Actualizada";
                    return RedirectToAction("Index");
                }
                return View();
            }catch(Exception)
            {
                TempData["ErrorMessage"] = "Erro ao actualizar categoria";
                return RedirectToAction("Edit",id);
            }
        }

        [Authorize]
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