using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Laboris.Models;
using Laboris.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Laboris.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _sigInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _sigInManager = signInManager;
        }

      

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm registerVM)
        {
           
            if (!ModelState.IsValid)
            {
                return View();
            }

            User user = new User()
            {
                UserName = registerVM.Email,
                Email = registerVM.Email,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                
            };

            var result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            await _sigInManager.SignInAsync(user,true);
            //burda true=> remember me

            return RedirectToAction("Login");

        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVM)
        {
            if (!ModelState.IsValid)
            {

                return View(loginVM);
            }
            User user;
            if (loginVM.Email.Contains("@"))
            {

                user = await _userManager.FindByEmailAsync(loginVM.Email);

            }
            else
            {
                user = await _userManager.FindByNameAsync(loginVM.Email);
            }
            if (user == null)
            {

                ModelState.AddModelError("", "Invalid Username Or Password!");
                return View();
            }
            var result = await _sigInManager.CheckPasswordSignInAsync(user, loginVM.Password, true);

            if (!result.Succeeded)
            {

                ModelState.AddModelError("", "Invalid Username Or Password!");
                return View();
            }

            if (result.IsLockedOut)
            {

                ModelState.AddModelError("", "Please Try Again Later!");
                return View();
            }
            await _sigInManager.SignInAsync(user, loginVM.RememberMe);
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Logout()
        {
            await _sigInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

