
using Azure.Storage.Blobs;
using Diplom2.Data;
using Diplom2.Models;
using Diplom2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diplom2.Controllers
{
    [Authorize(Policy = "Status")]
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        
        public AdminController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> IndexAsync()
        {
            List<AdminIndexView> model = new List<AdminIndexView>();
            var users = _userManager.Users.Include(u => u.Collections).ToList();
            foreach(var user in users)
            {
                model.Add(new AdminIndexView
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Email = user.Email,
                    Status = user.Status,
                    Role = await _userManager.GetRolesAsync(user),
                    QuantityCollectionsUser = user.Collections.Count(),
            });
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
            return View(user);
        }

       [HttpGet]
        public async Task<ActionResult> AddRole(string id)
        {
            if(id != null)
            {
                ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
                AdminAddRole role = new AdminAddRole
                {
                    User = user,
                    RoleUser = await _userManager.GetRolesAsync(user),
                    Role = _roleManager.Roles.ToList(),
            };
                return View(role);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddRolePost(string id, List<string> roles)
        {
            ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if(user != null)
            {
                var rolesUser = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                var addedRoles = roles.Except(rolesUser);
                var removedRoles = rolesUser.Except(roles);
                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);
                return RedirectToAction("Index");
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteUser(string id)
        {
            if (id != null)
            {
                var user = _userManager.Users.FirstOrDefault(u => u.Id == id);
                if (user != null)
                {
                    var currentUserId = user.Id;

                    string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
                    BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(currentUserId);
                    bool isExist = await containerClient.ExistsAsync();
                    if(isExist)
                        await containerClient.DeleteAsync();
                    await _userManager.DeleteAsync(user);
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }

        public RedirectToActionResult LockUser(string id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if(user != null)
            {
                user.Status = true;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public RedirectToActionResult UnlockUser(string id)
        {
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.Status = false;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AddTheme()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddTheme(string theme)
        {
            if(theme != null)
            {
                var newTheme = _context.Themes.FirstOrDefault(u => u.ThemeName == theme);
                if(newTheme == null)
                {
                    await _context.Themes.AddAsync(new Theme
                    {
                        ThemeName = theme,
                    });
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }

        [HttpGet]
        public ActionResult RemoveTheme()
        {
            var themes = _context.Themes.ToList();
            return View(themes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveTheme(List<string> themeremove)
        {
            var collections = await _context.Collections.ToListAsync();
            var allTheme = await _context.Themes.ToListAsync();
            foreach(var theme in themeremove)
            {
                var themeInCollection = collections.FirstOrDefault(u => u.Theme == theme);
                if(themeInCollection != null)
                {
                    ModelState.AddModelError(string.Empty, "Collections in theme " + theme);
                    return View(allTheme);
                }
                else
                {
                    var thisTheme = allTheme.FirstOrDefault(u => u.ThemeName == theme);
                    _context.Themes.Remove(thisTheme);
                    _context.SaveChanges();
                }

            }
            return NotFound();
        }
    }
}
