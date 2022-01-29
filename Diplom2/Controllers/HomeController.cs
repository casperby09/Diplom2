using Diplom2.Data;
using Diplom2.Models;
using Diplom2.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Diplom2.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<HomeController> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var collectionsTop = _context.Collections
                .Include(u => u.ApplicationUser)
                .Include(u => u.Items)
                .OrderByDescending(c => c.Items.Count())
                .ToList().Take(3);
            var itemsLast = _context.Items
                .Include(u => u.Tags)
                .Include(u => u.Likes)
                .Include(u => u.Collection)
                .ToList().Take(10);
            var tagsAll = _context.Tags.Where(u => u.Items.Count() > 0).ToList();

            HomeIndexView homeIndexView = new HomeIndexView
            {
                CollectionsIndex = collectionsTop,
                ItemsIndex = itemsLast,
                TagsIndex = tagsAll,
            };
            return View(homeIndexView);
        }

        // реализация не очень, переделать если будет время
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search(string search)
        {
            if (search == null)
                return RedirectToAction("Index", "Home");
            search = search.ToLower();
            var items = await _context.Items
                .Include(u => u.Collection)
                    .ThenInclude(u => u.ApplicationUser)
                .Include(u => u.ValueLines)
                .Include(u => u.Comments)
                .Include(u => u.ValueTexts)
                .ToListAsync();
            var itemsCollectionAndNameSearch = items.Where(u => u.Collection.ShortDescription.ToLower().Contains(search) || 
                                                                u.ItemName.ToLower().Contains(search) || 
                                                                u.Collection.CollectionName.ToLower().Contains(search) ||
                                                                u.Collection.ApplicationUser.UserName.ToLower().Contains(search)).ToList();
            List<Item> itemsListitemSearch = new List<Item>();
            foreach(var element in items)
            {
                var itemsComment = element.Comments.Where(u => u.Description.ToLower().Contains(search));
                if(itemsComment.Count() > 0)
                {
                    itemsListitemSearch.Add(element);
                    continue;
                } 
                var itemsText = element.ValueTexts.Where(u => u.Value.ToLower().Contains(search));
                if(itemsText.Count() > 0)
                {
                    itemsListitemSearch.Add(element);
                    continue;
                }
                var itemsLine = element.ValueLines.Where(u => u.Value.ToLower().Contains(search));
                if (itemsLine.Count() > 0)
                {
                    itemsListitemSearch.Add(element);
                    continue;
                }
            }
            List<Item> result = new List<Item>();
            result.AddRange(itemsCollectionAndNameSearch);
            result.AddRange(itemsListitemSearch.Except(result));
            ViewBag.Title = "search" + search;
            return View("SortTag", result);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [AllowAnonymous]
        public IActionResult SortTag(int tagid)
        {
            var items = _context.Tags
                .Include(u => u.Items)
                    .ThenInclude(u => u.Item)
                .FirstOrDefault(u => u.TagId == tagid)
                .Items.ToList();
            List<Item> result = new List<Item>();
            foreach (var item in items)
            {
                result.Add(item.Item);
            }
            if (items.Count > 0)
            {
                ViewBag.Title = items.First().Tag.Name;
                return View(result);
            }
            return RedirectToAction("Index", "Home");
        }

        public string LikeThis(int id)
        {
            var item = _context.Items
                .Include(u => u.Likes)
                    .ThenInclude(u => u.User)
                .FirstOrDefault(a => a.ItemId == id);
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var like = item.Likes.FirstOrDefault(u => u.User.Id == currentUserId);
            if (like == null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == currentUserId);
                    _context.Likes.Add(new Like
                    {
                        User = user,
                        Item = item,
                    });
                    _context.SaveChanges();
                }
            }

            return item.Likes.Count().ToString();
        }

        public string UnlikeThis(int id)
        {
            var item = _context.Items.Include(u => u.Likes).ThenInclude(u => u.User).FirstOrDefault(a => a.ItemId == id);
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var like = item.Likes.FirstOrDefault(u => u.User.Id == currentUserId);
            if (like != null)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var likeUser = item.Likes.FirstOrDefault(u => u.User.Id == currentUserId);
                    if (likeUser != null)
                    {
                        _context.Likes.Remove(likeUser);
                    }
                    _context.SaveChanges();
                }
            }
            return item.Likes.Count().ToString();
        }

        public async Task<bool> ThemeSite(bool themebool)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if(user == null)
                return false;
            if(themebool)
            {
                user.ThemeSite = true;
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            else
            {
                user.ThemeSite = false;
                await _signInManager.SignInAsync(user, isPersistent: false);
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
