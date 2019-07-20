using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Identity_example.Data;
using Identity_example.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Identity_example.ViewModels;
using static Identity_example.Utilities.SeedRoles;

namespace Identity_example.Controllers
{
    public class UsersController : Controller
    {
        #region Global variables
        private readonly UserIdentityDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        #endregion
        public UsersController(UserIdentityDbContext context,
                               UserManager<User> userManager,
                               SignInManager<User> signInManager,
                               RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            ViewBag.Countries = _context.Countries.ToList();
            return View();
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Countries = _context.Countries;
                return View(registerViewModel);
            }
            User newUser = new User
            {
                CityID = registerViewModel.CityID,
                Firstname = registerViewModel.Firstname,
                Lastname = registerViewModel.Lastname,
                Email = registerViewModel.Email,
                UserName = registerViewModel.Username
            };
            IdentityResult identityResult =  await _userManager.CreateAsync(newUser, registerViewModel.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                ViewBag.Countries = _context.Countries;
                return View(registerViewModel);
            }
            await _userManager.AddToRoleAsync(newUser, Roles.Member.ToString());
            await _signInManager.SignInAsync(newUser, true);
            return RedirectToAction("Index","Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            User user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if (user==null)
            {
                ModelState.AddModelError("", "E-poçt və ya şifrə yalnışdır.");
                return View(loginViewModel);
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, true);

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "E-poçt və ya şifrə yalnışdır.");
                return View(loginViewModel);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            
            return RedirectToAction(nameof(HomeController.Index),"Home");
        }
        [HttpPost]
        public IActionResult LoadCities(int countryID)
        {
            return PartialView("_SelectCities", _context.Cities.Where(c => c.CountryID == countryID));
        }

        #region Seed roles
        public async Task SeedRoles()
        {
            if (!await _roleManager.RoleExistsAsync(Roles.Admin.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            }

            if (!await _roleManager.RoleExistsAsync(Roles.Manager.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Manager.ToString()));
            }

            if (!await _roleManager.RoleExistsAsync(Roles.Member.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Member.ToString()));
            }
        }
        #endregion
    }
}