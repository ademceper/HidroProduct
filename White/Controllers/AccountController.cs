using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using White.Models;
using System.Security.Claims;
using System.Security.Policy;
using White.Data;

namespace White.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;
        private readonly IWebHostEnvironment env;

        public AccountController(
            SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            IEmailService emailService,
            IWebHostEnvironment env
            )
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.emailService = emailService;
            this.env = env;
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View(new LoginViewModel { IsPersistent = true });
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, model.IsPersistent, true);
            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl ?? "/");
            }
            else
            {
                ModelState.AddModelError("", "Geçersiz kullanıcı girişi!");
                return View(model);
            }


        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Redirect("/");
        }

    }
}
        


        

       
        
        
        
           



        

    
