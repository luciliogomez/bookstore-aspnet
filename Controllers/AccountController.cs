using Bookstore.Models;
using Bookstore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
namespace Bookstore.Controllers{
    
    public class AccountController : Controller{

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register(string returnUrl = null)
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            registerViewModel.ReturnUrl = returnUrl;
            return View(registerViewModel);
        }

        public IActionResult Login(string returnUrl = null)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            loginViewModel.ReturnUrl = returnUrl;
            return View(loginViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            try{
                if(ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
                    if(user != null){
                        var result  = await _signInManager.CheckPasswordSignInAsync(user,loginViewModel.Password,false);
                        if(result.Succeeded)
                        {
                            Console.WriteLine("CHECKPOINT: PASSWORD CHECHED");
                            // await _signInManager.SignInAsync(user, isPersistent: false);
                                var claims = new[] 
{ 
    new Claim(ClaimTypes.Name, user.UserName)
};

    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                            return RedirectToAction("Index","Home");
                        }
                        if(result.IsNotAllowed)
                        {
                            ModelState.AddModelError("Email","Not Allowed");
                        }
                        
                    }else{
                        ModelState.AddModelError("Email","User Not Found");
                        return View(loginViewModel);
                    }
                    
                    
                }
                return View(loginViewModel);
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return NotFound();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            // await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home");

        }
    }
}