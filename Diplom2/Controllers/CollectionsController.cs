using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Diplom2.Data;
using Diplom2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Diplom2.Controllers
{
    public class CollectionsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _env;

        public CollectionsController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context; 
            _env = env;
        }



        // GET: CollectionsController
        [Authorize]
        public ActionResult Index(string id)
        {
            string currentUserId;
            if (User.IsInRole("admin") && id != null)
            {
                currentUserId = id; 
            }
            else
            {
                currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            ViewBag.UserId =  currentUserId;
            var collectionsUser = _context.Collections.Where(c => c.ApplicationUser.Id == currentUserId).ToList();
            return View(collectionsUser);
        }

        [AllowAnonymous]
        // GET: CollectionsController/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                //var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var collection = await _context.Collections.Include(u=> u.ApplicationUser).FirstOrDefaultAsync(u => u.CollectionId == id);
                ViewBag.UserName = collection.ApplicationUser.UserName;
            }
            
            ViewBag.Id = id;
            var itemsCollection = _context.Items.Where(c=> c.Collection.CollectionId == id);
            return View(itemsCollection);
        }

        // GET: CollectionsController/Create
        [Authorize]
        public ActionResult Create(string id)
        {
            ViewBag.UserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (id != null)
                ViewBag.UserId = id;
            IEnumerable<Theme> themes = _context.Themes;
            return View(themes);
        }

        // POST: CollectionsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(string id, IFormCollection formCollection)     
        {
            var file = formCollection.Files[0];
            string fileName = file.FileName;
            string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
            
            var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (id != null)
                currentUserId = id;

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(currentUserId);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
            List<string> filesNameList = new List<string>();

            await foreach(BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                filesNameList.Add(blobItem.Name);
            }
            while (filesNameList.Contains(fileName))
            {
                fileName = "new_" + fileName;
            }
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            BlobHttpHeaders httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };
            
            string url = blobClient.Uri.ToString();

            Collection collection = await _context.Collections.FirstOrDefaultAsync(u => u.CollectionName == formCollection["collectionName"].ToString());
            if (collection == null)
            {
                ApplicationUser User = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == currentUserId);
                ICollection<Number> numbers = new List<Number>();
                ICollection<Line> lines = new List<Line>();
                ICollection<Text> texts = new List<Text>();
                ICollection<Date> dates = new List<Date>();
                ICollection<Logical> logicals = new List<Logical>();
                
                foreach (var element in formCollection)
                {
                    if(element.Key.Contains("number"))
                    {
                        Number number = new Number
                        {
                            Name = element.Value,
                        };
                        numbers.Add(number);
                    }
                    else if (element.Key.Contains("line"))
                    {
                        Line line = new Line
                        {
                            Name = element.Value,
                        };
                        lines.Add(line);
                    }
                    else if (element.Key.Contains("text"))
                    {
                        Text text = new Text
                        {
                            Name = element.Value,
                        };
                        texts.Add(text);
                    }
                    else if (element.Key.Contains("date"))
                    {
                        Date date = new Date
                        {
                            Name = element.Value,
                        };
                        dates.Add(date);
                    }
                    else if (element.Key.Contains("logical"))
                    {
                        Logical logical = new Logical
                        {
                            Name = element.Value,
                        };
                        logicals.Add(logical);
                    }
                }
                await _context.Collections.AddAsync(
                    new Collection
                    {
                        CollectionName = formCollection["collectionName"].ToString(),
                        ShortDescription = formCollection["shortDescription"].ToString(),
                        Theme = formCollection["theme"].ToString(),
                        ApplicationUser = User,
                        PhotoURL = url,
                        Numbers = numbers.ToList(),
                        Lines = lines.ToList(),
                        Texts = texts.ToList(),
                        Dates = dates.ToList(),
                        Logicals = logicals.ToList(),
                    });
                
                await _context.SaveChangesAsync();
                await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders);
            }
            return RedirectToAction("Index", "Home");
        }
        
        // GET: CollectionsController/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Index");
            Collection collection = await _context.Collections
                .Include(u => u.ApplicationUser)
                .Include(u => u.Numbers)
                .Include(u => u.Lines)
                .Include(u => u.Texts)
                .Include(u => u.Dates)
                .Include(u => u.Logicals)
                .FirstOrDefaultAsync(u => u.CollectionId == id);
            if(collection == null)
                return RedirectToAction("Index");
            if (collection.ApplicationUser.Id != this.User.FindFirst(ClaimTypes.NameIdentifier).Value && !this.User.IsInRole("admin"))
                return RedirectToAction("Index", "Home");
            ViewBag.Themes = _context.Themes.ToList();
            
            return View(collection);
        }

        // POST: CollectionsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, IFormCollection collection)
        {
            if (id == null)
                return RedirectToAction(nameof(Index));
            Collection collectionUser = await _context.Collections
                .Include(u => u.ApplicationUser)
                .Include(u => u.Numbers)
                .Include(u => u.Lines)
                .Include(u => u.Texts)
                .Include(u => u.Dates)
                .Include(u => u.Logicals)
                .FirstOrDefaultAsync(u => u.CollectionId == id);
            if(collectionUser == null)
                return RedirectToAction(nameof(Index));
            ICollection<Number> numbers = new List<Number>();
            ICollection<Line> lines = new List<Line>();
            ICollection<Text> texts = new List<Text>();
            ICollection<Date> dates = new List<Date>();
            ICollection<Logical> logicals = new List<Logical>();
            foreach (var item in collection)
            {
                if (item.Key.Contains("Number_"))
                {
                    string[] nameAndId = item.Key.Split(new char[] { '_' });
                    int itemId = int.Parse(nameAndId[1]);
                    Number number = collectionUser.Numbers.FirstOrDefault(u => u.Id == itemId);
                    if(item.Value != number.Name)
                        number.Name = item.Value;
                    numbers.Add(number);
                }
                else if (item.Key.Contains("Line_"))
                {
                    string[] nameAndId = item.Key.Split(new char[] { '_' });
                    int itemId = int.Parse(nameAndId[1]);
                    Line line = collectionUser.Lines.FirstOrDefault(u => u.Id == itemId);
                    if (item.Value != line.Name)
                        line.Name = item.Value;
                    lines.Add(line);
                }
                else if (item.Key.Contains("Text_"))
                {
                    string[] nameAndId = item.Key.Split(new char[] { '_' });
                    int itemId = int.Parse(nameAndId[1]);
                    Text text = collectionUser.Texts.FirstOrDefault(u => u.Id == itemId);
                    if (item.Value != text.Name)
                        text.Name = item.Value;
                    texts.Add(text);
                }
                else if (item.Key.Contains("Date_"))
                {
                    string[] nameAndId = item.Key.Split(new char[] { '_' });
                    int itemId = int.Parse(nameAndId[1]);
                    Date date = collectionUser.Dates.FirstOrDefault(u => u.Id == itemId);
                    if (item.Value != date.Name)
                        date.Name = item.Value;
                    dates.Add(date);
                }
                else if (item.Key.Contains("Logical_"))
                {
                    string[] nameAndId = item.Key.Split(new char[] { '_' });
                    int itemId = int.Parse(nameAndId[1]);
                    Logical logical = collectionUser.Logicals.FirstOrDefault(u => u.Id == itemId);
                    if (item.Value != logical.Name)
                        logical.Name = item.Value;
                    logicals.Add(logical);
                }
                else if (item.Key.Contains("number"))
                {
                    Number number = new Number
                    {
                        Name = item.Value,
                    };
                    numbers.Add(number);
                }
                else if (item.Key.Contains("line"))
                {
                    Line line = new Line
                    {
                        Name = item.Value,
                    };
                    lines.Add(line);
                }
                else if (item.Key.Contains("text"))
                {
                    Text text = new Text
                    {
                        Name = item.Value,
                    };
                    texts.Add(text);
                }
                else if (item.Key.Contains("date"))
                {
                    Date date = new Date
                    {
                        Name = item.Value,
                    };
                    dates.Add(date);
                }
                else if (item.Key.Contains("logical"))
                {
                    Logical logical = new Logical
                    {
                        Name = item.Value,
                    };
                    logicals.Add(logical);
                }
                else if (item.Key.Contains("collectionName"))
                {
                    collectionUser.CollectionName = item.Value;
                }
                else if(item.Key.Contains("shortDescription"))
                {
                    collectionUser.ShortDescription = item.Value;
                }
                else if(item.Key.Contains("theme"))
                {
                    collectionUser.Theme = item.Value;
                }

            }
            if(collection.Files.Count != 0)
            {
                var currentUserId = collectionUser.ApplicationUser.Id;
                var file = collection.Files[0];
                string fileName = file.FileName;
                string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(currentUserId);
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
                List<string> filesNameList = new List<string>();
                await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
                {
                    filesNameList.Add(blobItem.Name);
                }
                while (filesNameList.Contains(fileName))
                {
                    fileName = "new_" + fileName;
                }
                BlobClient blobClient = containerClient.GetBlobClient(fileName);
                BlobHttpHeaders httpHeaders = new BlobHttpHeaders()
                {
                    ContentType = file.ContentType
                };
                await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders);
                string url = blobClient.Uri.ToString();

                string deleteImage = collectionUser.PhotoURL;
                string[] listURL = deleteImage.Split(new char[] { '/' });
                string deleteFileName = listURL.Last();
                await containerClient.DeleteBlobAsync(deleteFileName);
            }
            collectionUser.Numbers = numbers.ToList();
            collectionUser.Lines = lines.ToList();
            collectionUser.Texts = texts.ToList();
            collectionUser.Dates = dates.ToList();
            collectionUser.Logicals = logicals.ToList();
            _context.Attach(collectionUser).State = EntityState.Modified;
            try
            {
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.Themes = _context.Themes.ToList();
                return View(collectionUser);
            }
        }

        // GET: CollectionsController/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Home");
            Collection collection = await _context.Collections.Include(u => u.ApplicationUser).FirstOrDefaultAsync(u => u.CollectionId == id);
            if(collection == null)
                return RedirectToAction("Index", "Home");
            if (collection.ApplicationUser.Id != this.User.FindFirst(ClaimTypes.NameIdentifier).Value && !this.User.IsInRole("admin"))
                return RedirectToAction("Index", "Home");
            return View(collection);
        }

        // POST: CollectionsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteCollection(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Home");
            Collection collection = await _context.Collections.Include(u => u.ApplicationUser).FirstOrDefaultAsync(u => u.CollectionId == id);
            if(collection != null)
            {
                var currentUserId = collection.ApplicationUser.Id;
                string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(currentUserId);
                string deleteImage = collection.PhotoURL;
                string[] listURL = deleteImage.Split(new char[] { '/' });
                string deleteFileName = listURL.Last();
                await containerClient.DeleteBlobAsync(deleteFileName);
                _context.Collections.Remove(collection);
            }
            try
            {
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                
                return View();
            }
        }
    }
}
