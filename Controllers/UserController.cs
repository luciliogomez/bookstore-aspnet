using Bookstore.Data;
using Bookstore.Models;
using Bookstore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Controllers{
    
    public class UserController : Controller{

        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _db;
       

        public UserController(UserManager<AppUser> userManager,ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
            _userManager = userManager;
        }

        
        public  IActionResult Index()
        {
            try{
                var users = _db.AppUsers.ToList();
                return View(users);
            }catch(Exception e)
            {
                Console.WriteLine("ERROR"+e.Message);
                Console.WriteLine("ERROR"+e.InnerException.Message);
                return  NotFound();
            }
        }
        public IActionResult Create()
        {
            return  View();
        }

         [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel createViewModel)
        {
            try{
                if(ModelState.IsValid)
                {
                    var user = new AppUser{Email = createViewModel.Email, UserName = createViewModel.UserName};
                    var result = await _userManager.CreateAsync(user,"#aB123");
                    if(result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "Utilizador Adicionado";
                        return RedirectToAction("Index");
                    }
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("Email",error.Description);    
                    }
                }
                return View(createViewModel);

            }catch(Exception)
            {
                TempData["ErrorMessage"] = "Ocorreu um problema tente outra vez";
                return RedirectToAction("Index");
            }
        }
    }
}