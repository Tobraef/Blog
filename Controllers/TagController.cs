using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Blog.Models;

namespace Blog.Controllers
{
    public class TagController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            using (var db = new BlogContext())
            {
                var allTags = db.Tags
                    .Include(x => x.Entries)
                    .Include(x => x.TagToUsers)
                    .ToList();
                var viewModel = new TagInfo
                {
                    AllTags = allTags,
                    TagToPopularity = allTags
                        .Select(e => new KeyValuePair<EntryTag, int>(e, e.TagToUsers.Sum(ttu => ttu.TimesSeen)))
                        .OrderByDescending(kvp => kvp.Value),
                    TagToUses = allTags
                        .Select(e => new KeyValuePair<EntryTag, int>(e, e.Entries.Count()))
                        .OrderByDescending(kvp => kvp.Value)
                };
                return View(viewModel);
            }
        }

        [Route("Edit/{id}")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (var db = new BlogContext())
            {
                return View(db.Tags.Find(id));
            }
        }

        [HttpPost]
        public ActionResult Edit(EntryTag tag)
        {
            using (var db = new BlogContext())
            {
                db.Tags.Find(tag.TagId).Name = tag.Name;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}