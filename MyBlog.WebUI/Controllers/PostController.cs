using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Abscract;
using MyBlog.Data.EfCore;
using MyBlog.Entity;
using MyBlog.WebUI.Models;
using P.Pager;

namespace MyBlog.WebUI.Controllers
{
    [AllowAnonymous]
    public class PostController : Controller
    {
        private IPostRepository _repository;
        private ICategoriesRepository _categories;
        private readonly UserManager<AppUser> _userManager;

        public PostController(IPostRepository repository, ICategoriesRepository categories, UserManager<AppUser> userManager)
        {
            _repository = repository;
            _categories = categories;
            _userManager = userManager;
        }

        public IActionResult Index(int page)
        {
            ViewBag.isUserPosts = false;
            ViewBag.isSearchPosts = false;
            var model = _repository.GetAll().Include(x => x.User).Include(x => x.Category).OrderByDescending(x => x.PublishDate).ToPagerList(page, 8);

            return View(model);
        }

        [Authorize]
        public IActionResult NewPost()
        {
            List<Categories> listCategories = _categories.GetAll().ToList();

            var model = new NewPostModel()
            {
                Categories = listCategories
            };
            ViewBag.NewPostViewAction = "NewPost";
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> NewPost(NewPostModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var category = _categories.GetById(model.CategoryId);

                Post post = new Post
                {
                    Title = model.Title,
                    CategoryId = model.CategoryId,
                    Category = category,
                    Content = model.Content,
                    Summary = model.Summary,
                    Slug = model.Slug,
                    User = user,
                    UserId = user.Id
                };

                _repository.Add(post);
                _repository.Save();
                return RedirectToAction("Index", "Post");
            }
            return View();
        }

        [Route("Post/Details/{slug}")]
        public IActionResult Details(string slug)
        {
            Post post = _repository.GetBySlug(slug);
            post.Category = _categories.GetById(post.CategoryId);
            return View(post);
        }

        [Authorize]
        [Route("{username}/posts/{slug}")]
        public IActionResult Edit(string slug)
        {
            List<Categories> listCategories = _categories.GetAll().ToList();
            Post post = _repository.GetBySlug(slug);
            var model = new NewPostModel()
            {
                CategoryId = post.CategoryId,
                Content = post.Content,
                Id = post.Id,
                Slug = post.Slug,
                Title = post.Title,
                Categories = listCategories
            };

            ViewBag.NewPostViewAction = "Edit";
            return View("NewPost", model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(NewPostModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var category = _categories.GetById(model.CategoryId);

                Post post = new Post
                {
                    Title = model.Title,
                    CategoryId = model.CategoryId,
                    Category = category,
                    Content = model.Content,
                    Slug = model.Slug,
                    User = user,
                    UserId = user.Id
                };

                _repository.Update(post);
                _repository.Save();
                return RedirectToAction("Index", "Post");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var post = _repository.GetById(id);
            _repository.Delete(post);
            _repository.Save();
            return RedirectToAction("Profile", "Account");
        }

        [Route("{username?}/posts")]
        [Route("{categoryName?}/posts")]
        public async Task<IActionResult> UserPost(int page, string username, string categoryName)
        {
            IPager<Post> model = null;
            if (username != null)
            {
                var user = await _userManager.FindByNameAsync(username);
                model = _repository.GetAll().Where(x => x.UserId == user.Id).Include(x => x.User).Include(x => x.Category).OrderByDescending(x => x.PublishDate).ToPagerList(page, 8);

            }
            else
            {
                var category = _categories.GetAll().Where(x => x.Name == categoryName).FirstOrDefault();
                model = _repository.GetAll().Where(x => x.CategoryId == category.Id).Include(x => x.User).Include(x => x.Category).OrderByDescending(x => x.PublishDate).ToPagerList(page, 8);
                ViewBag.isCategoryPosts = true;
            }
            ViewBag.isUserPosts = true;
            return View("Index", model);
        }


        public IActionResult Search(string q, int page)
        {
            if (!string.IsNullOrWhiteSpace(q))
            {
                var model = _repository.GetAll().Where(x => x.Title.Contains(q) || x.Content.Contains(q)).Include(x => x.User).Include(x => x.Category).OrderByDescending(x => x.PublishDate).ToPagerList(page, 8);
                ViewBag.isSearchPosts = true;
                if (model.Count()>0)
                {
                    return View(model);
                }
                else
                {
                    return NotFound(q+" is not found!");
                }
                
            }
            return NotFound("Search input empty!");
        }
    }
}