using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using skcyDMSCataloguing.ViewModels;

namespace skcyDMSCataloguing.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

       [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
            // create (register) a user to the persistent dbstore
               var user = new IdentityUser {UserName = model.Email, Email = model.Email};
               var result= await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // sign in the created user creating a session cookie (isPersistent:false)
                    await signInManager.SignInAsync(user, isPersistent:false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description); //if there is an erro it will be displayed via the corresponding view validation check
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {                                 
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, 
                                        model.RememberMe, false);
                /* IsPersistent is the third argument of PasswordSignInAsync and takes its value
                 * from the user data entry in rememberme check box (if checked=yes).
                 * false argument stands for negate user lockout after a certain time of attempts  */


                if (result.Succeeded)
                {                                         
                    return RedirectToAction("Index", "Home");
                }

                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt"); 
                    /* if there is an error it will be displayed via the corresponding 
                     * view validation check. there is no need here to loop over errors as 
                     * it was the case in register method */              
            }
            return View(model);
        }
    }
}
