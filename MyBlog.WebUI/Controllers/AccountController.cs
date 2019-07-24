using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Abscract;
using MyBlog.Entity;
using MyBlog.WebUI.Models;
using P.Pager;

namespace MyBlog.WebUI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IPostRepository _repository;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IPostRepository repository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repository = repository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser()
                {
                    Name = model.Name,
                    LastName = model.LastName,
                    UserName = model.Username,
                };

                var result = await _userManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user,false);
                    return RedirectToAction("Index", "Post");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    var result =await _signInManager.PasswordSignInAsync(user, model.Password,false,false);

                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl );
                    }
                }
                ModelState.AddModelError("Password","Invalid username or password");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Post");
        }
        
        public async Task<IActionResult> Profile(int page)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var model = _repository.GetAll().Where(x => x.UserId == user.Id).Include(x => x.User).Include(x => x.Category).OrderByDescending(x => x.PublishDate).ToPagerList(page, 8);
            return View(model);
        }
    }
    
}