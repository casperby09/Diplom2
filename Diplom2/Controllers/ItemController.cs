using Diplom2.Data;
using Diplom2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Diplom2.Controllers
{
    public class ItemController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _env;
        public ItemController(
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

        [AllowAnonymous]
        // GET: ItemController/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.User = _context.ApplicationUsers.FirstOrDefault(u=> u.UserName == User.Identity.Name);
            Item item = _context.Items
                .Include(u=> u.Collection)
                    .ThenInclude(u=> u.ApplicationUser)
                .Include(u => u.ValueNumbers)
                    .ThenInclude(u => u.Number)
                .Include(u => u.ValueLines)
                    .ThenInclude(u => u.Line)
                .Include(u => u.ValueTexts)
                    .ThenInclude(u => u.Text)
                .Include(u => u.ValueDates)
                    .ThenInclude(u => u.Date)
                .Include(u => u.ValueLogicals)
                    .ThenInclude(u => u.Logical)
                .Include(u => u.Tags)
                    .ThenInclude(u => u.Tag)
                .Include(u=> u.Likes)
                    .ThenInclude(u=> u.User)
                .Include(u=> u.Comments)
                    .ThenInclude(u=> u.User)
                .Where(u => u.ItemId == id).FirstOrDefault();
            return View(item);
        }

        [Authorize]
        // GET: ItemController/Create
        public async Task<ActionResult> Create(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "Home");
            ICollection<Tag> tags = _context.Tags.ToList();
            List<string> tagNames = new List<string>();
            foreach (Tag tag in tags)
            {
                tagNames.Add(tag.Name);
            };
            ViewBag.TagNames = tagNames;
            Collection collection = await _context.Collections
                .Include(u => u.ApplicationUser)
                .Include(u => u.Numbers)
                .Include(u => u.Lines)
                .Include(u => u.Texts)
                .Include(u => u.Dates)
                .Include(u => u.Logicals)
                .FirstOrDefaultAsync(c => c.CollectionId == id);
            if (collection == null || collection.ApplicationUser.Id != this.User.FindFirst(ClaimTypes.NameIdentifier).Value && !this.User.IsInRole("admin"))
                return RedirectToAction("Index", "Home");
            return View(collection);
        }

        // POST: ItemController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection, int idCollection)
        {
            Collection userCollection = await _context.Collections
                        .Include(u => u.Numbers)
                        .Include(u => u.Lines)
                        .Include(u => u.Texts)
                        .Include(u => u.Dates)
                        .Include(u => u.Logicals)
                        .FirstOrDefaultAsync(u => u.CollectionId == idCollection);
            try
            {
                Item item = await _context.Items.FirstOrDefaultAsync(u => u.ItemName == collection["itemName"].ToString());
                if (item == null)
                {
                    List<Logical> logicals = userCollection.Logicals.ToList();
                    ICollection<ValueNumber> valueNumbers = new List<ValueNumber>();
                    ICollection<ValueLine> valueLines = new List<ValueLine>();
                    ICollection<ValueText> valueTexts = new List<ValueText>();
                    ICollection<ValueDate> valueDates = new List<ValueDate>();
                    ICollection<ValueLogical> valueLogicals = new List<ValueLogical>();
                    ICollection<Tag> allTags = _context.Tags.ToList();
                    ICollection<TagItem> itemTagsItem = new List<TagItem>();

                    foreach (Logical logical in logicals)
                    {
                        ValueLogical valueLogical = new ValueLogical
                        {
                            Value = false,
                            Logical = logical
                        };
                        valueLogicals.Add(valueLogical);
                    };
                    foreach (var element in collection)
                    {
                        if (element.Key.Contains("number"))
                        {
                            string[] nameAndId = element.Key.Split(new char[] { '_' });
                            int numberId = int.Parse(nameAndId[1]);
                            Number number = userCollection.Numbers.FirstOrDefault(u => u.Id == numberId);
                            ValueNumber valueNumber = new ValueNumber
                            {
                                Value = double.Parse(element.Value),
                                Number = number,
                            };
                            valueNumbers.Add(valueNumber);
                        }
                        else if (element.Key.Contains("line"))
                        {
                            string[] nameAndId = element.Key.Split(new char[] { '_' });
                            int lineId = int.Parse(nameAndId[1]);
                            Line line = userCollection.Lines.FirstOrDefault(u => u.Id == lineId);
                            ValueLine valueLine = new ValueLine
                            {
                                Value = element.Value,
                                Line = line,
                            };
                            valueLines.Add(valueLine);
                        }
                        else if (element.Key.Contains("text"))
                        {
                            string[] nameAndId = element.Key.Split(new char[] { '_' });
                            int textId = int.Parse(nameAndId[1]);
                            Text text = userCollection.Texts.FirstOrDefault(u => u.Id == textId);
                            ValueText valueText = new ValueText
                            {
                                Value = element.Value,
                                Text = text,

                            };
                            valueTexts.Add(valueText);
                        }
                        else if (element.Key.Contains("date"))
                        {
                            string[] nameAndId = element.Key.Split(new char[] { '_' });
                            int dateId = int.Parse(nameAndId[1]);
                            Date date = userCollection.Dates.FirstOrDefault(u => u.Id == dateId);
                            ValueDate valueDate = new ValueDate
                            {
                                Value = DateTime.Parse(element.Value),
                                Date = date,
                            };
                            valueDates.Add(valueDate);
                        }
                        else if (element.Key.Contains("logical"))
                        {
                            string[] nameAndId = element.Key.Split(new char[] { '_' });
                            int logicalId = int.Parse(nameAndId[1]);
                            Logical logical = userCollection.Logicals.FirstOrDefault(u => u.Id == logicalId);
                            ValueLogical valueLogical = valueLogicals.FirstOrDefault(a => a.Logical == logical);
                            valueLogical.Value = true;
                        }
                        else if (element.Key.Contains("tags[]"))
                        {
                            string[] tags = element.Value.ToString().Split(new char[] { ',' });
                            foreach(string tagelement in tags)
                            {
                                Tag tag = allTags.FirstOrDefault(u => u.Name == tagelement);
                                if (tag == null)
                                {
                                    Tag itemTag = new Tag
                                    {
                                        Name = tagelement,
                                    };
                                    TagItem tagItem = new TagItem
                                    {
                                        Tag = itemTag,
                                    };
                                    itemTagsItem.Add(tagItem);
                                }
                                else
                                {
                                    TagItem tagItem = new TagItem
                                    {
                                        Tag = tag,
                                    };
                                    itemTagsItem.Add(tagItem);
                                }
                            }
                            
                        }
                    }
                    _context.Items.Add(new Item
                    {
                        ItemName = collection["itemName"].ToString(),
                        Collection = userCollection,
                        ValueNumbers = valueNumbers.ToList(),
                        ValueLines = valueLines.ToList(),
                        ValueTexts = valueTexts.ToList(),
                        ValueDates = valueDates.ToList(),
                        ValueLogicals = valueLogicals.ToList(),
                        Tags = itemTagsItem.ToList(),
                    });
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Details", "Collections", new {id = idCollection});
            }
            catch
            {
                return View(userCollection);
            }
        }

        // GET: ItemController/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Item item = _context.Items
                .Include(u => u.Collection)
                    .ThenInclude(u => u.ApplicationUser)
                .Include(u => u.Tags)
                    .ThenInclude(u => u.Tag)
                .Include(u => u.ValueNumbers)
                    .ThenInclude(u => u.Number)
                .Include(u => u.ValueLines)
                    .ThenInclude(u => u.Line)
                .Include(u => u.ValueTexts)
                    .ThenInclude(u => u.Text)
                .Include(u => u.ValueDates)
                    .ThenInclude(u => u.Date)
                .Include(u => u.ValueLogicals)
                    .ThenInclude(u => u.Logical)
                .FirstOrDefault(u=> u.ItemId == id);
            if (item != null)
            {
                string tagsItem = "";
                foreach(var tag in item.Tags)
                {
                    if (tag == item.Tags.Last())
                    {
                        tagsItem = tagsItem + tag.Tag.Name;
                        continue;
                    }  
                    tagsItem = tagsItem + tag.Tag.Name + ", ";
                }
                ViewBag.TagsItem = tagsItem;
                var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (item.Collection.ApplicationUser.Id == currentUserId || this.User.IsInRole("admin"))
                {
                    ICollection<Tag> tags = _context.Tags.ToList();
                    List<string> tagNames = new List<string>();
                    foreach (Tag tag in tags)
                    {
                        tagNames.Add(tag.Name);
                    };
                    ViewBag.TagNames = tagNames;
                    return View(item);
                }
                return RedirectToAction("Details", "Item", new { id = item.ItemId });
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: ItemController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditItemAsync(int id, IFormCollection collection)
        {
            try
            {
                Item item = _context.Items
                    .Include(u => u.Collection)
                        .ThenInclude(u => u.Items)
                    .Include(u => u.Tags)
                        .ThenInclude(u => u.Tag)
                    .Include(u => u.ValueNumbers)
                        .ThenInclude(u => u.Number)
                    .Include(u => u.ValueLines)
                        .ThenInclude(u => u.Line)
                    .Include(u => u.ValueTexts)
                        .ThenInclude(u => u.Text)
                    .Include(u => u.ValueDates)
                        .ThenInclude(u => u.Date)
                    .Include(u => u.ValueLogicals)
                        .ThenInclude(u => u.Logical)
                    .FirstOrDefault(u => u.ItemId == id);

                ICollection<ValueLogical> valueLogicalsItem = item.ValueLogicals.ToList();
                foreach(var valueLogical in valueLogicalsItem)
                    valueLogical.Value = false;

                ICollection<Tag> allTags = _context.Tags.ToList();
                ICollection<TagItem> itemTagsItem = new List<TagItem>();
                foreach (var element in collection)
                {
                    if (element.Key.Contains("tags[]"))
                    {
                        string[] tags = element.Value.ToString().Split(new char[] { ',' });
                        foreach (string tagelement in tags)
                        {
                            Tag tag = allTags.FirstOrDefault(u => u.Name == tagelement);
                            if (tag == null)
                            {
                                Tag itemTag = new Tag
                                {
                                    Name = tagelement,
                                };
                                TagItem tagItem = new TagItem
                                {
                                    Tag = itemTag,
                                };
                                itemTagsItem.Add(tagItem);
                            }
                            else
                            {
                                TagItem tagItem = new TagItem
                                {
                                    Tag = tag,
                                };
                                itemTagsItem.Add(tagItem);
                            }
                        }
                    }
                    else if (element.Key.Contains("valueNumber"))
                    {
                        string[] nameAndId = element.Key.Split(new char[] { '_' });
                        int idValueNumber = int.Parse(nameAndId[1]);
                        ValueNumber valueNumber = item.ValueNumbers.FirstOrDefault(u => u.Id == idValueNumber);
                        if (valueNumber.Value != double.Parse(element.Value))
                            valueNumber.Value = double.Parse(element.Value); 
                    }
                    else if (element.Key.Contains("valueLine"))
                    {
                        string[] nameAndId = element.Key.Split(new char[] { '_' });
                        int idValueLine = int.Parse(nameAndId[1]);
                        ValueLine valueLine = item.ValueLines.FirstOrDefault(u => u.Id == idValueLine);
                        if(valueLine.Value != element.Value)
                            valueLine.Value = element.Value;
                    }
                    else if (element.Key.Contains("valueText"))
                    {
                        string[] nameAndId = element.Key.Split(new char[] { '_' });
                        int idValueText = int.Parse(nameAndId[1]);
                        ValueText valueText = item.ValueTexts.FirstOrDefault(u => u.Id == idValueText);
                        if(valueText.Value != element.Value)
                            valueText.Value = element.Value; 
                    }
                    else if (element.Key.Contains("valueDate"))
                    {
                        string[] nameAndId = element.Key.Split(new char[] { '_' });
                        int idValueDate = int.Parse(nameAndId[1]);
                        ValueDate valueDate = item.ValueDates.FirstOrDefault(u => u.Id ==idValueDate);
                        if (valueDate.Value != DateTime.Parse(element.Value))
                            valueDate.Value = DateTime.Parse(element.Value);
                    }
                    else if(element.Key.Contains("valueLogical"))
                    {
                        string[] nameAndId = element.Key.Split(new char[] { '_' });
                        int idValueLogical = int.Parse(nameAndId[1]);
                        ValueLogical valueLogical = item.ValueLogicals.FirstOrDefault(u =>u.Id ==idValueLogical);
                        valueLogical.Value = true;
                    }
                    else if(element.Key.Contains("itemName"))
                    {
                        ICollection<Item> items = item.Collection.Items.ToList();
                        List<string> nameItems = new List<string>();
                        foreach(Item itemItem in items)
                            nameItems.Add(itemItem.ItemName);
                        if (item.ItemName != element.Value)
                        {
                            if (!nameItems.Contains(element.Value))
                                item.ItemName = element.Value;
                            else
                                return BadRequest("Error Name Item");
                        }  
                    }
                }
                item.Tags = itemTagsItem.ToList();
                _context.Attach(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Item", new { id = item.ItemId });
            }
            catch
            {
                return View();
            }
        }

        // GET: ItemController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            Item item = _context.Items.Include(u => u.Collection).ThenInclude(u => u.ApplicationUser).FirstOrDefault(u => u.ItemId == id);
            if (item == null)
                return BadRequest("Error No Item ID-" + id);
            if(item.Collection.ApplicationUser.Id != this.User.FindFirst(ClaimTypes.NameIdentifier).Value && !this.User.IsInRole("admin"))
                return RedirectToAction("Index", "Home");
            return View(item);
        }

        // POST: ItemController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteItem(int id)
        {
            Item item = _context.Items.Include(u => u.Collection).ThenInclude(u => u.ApplicationUser).FirstOrDefault(u => u.ItemId == id);
            if (item == null)
                return BadRequest("Error No Item ID-" + id);
            _context.Items.Remove(item);
            try
            {
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View(item);
            }
        }

        [Authorize]
        public string SendComment(string userName, int itemId, string text)
        {
            ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(u => u.UserName == userName);
            Item item = _context.Items.FirstOrDefault(u => u.ItemId == itemId);
            _context.Comments.Add(new Models.Comment
            {
                Description = text,
                DateTime = DateTime.Now,
                User = user,
                Item = item,
            });
            _context.SaveChanges();
            string date = DateTime.Now.ToString();
            return date;
        }
    }
}
