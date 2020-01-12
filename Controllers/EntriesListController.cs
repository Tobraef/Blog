using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Data.Entity;

namespace Blog.Controllers
{
    public class EntriesListController : Controller
    {
        public ActionResult ListOfEntries(string toFindString)
        {
            using (var db = new BlogContext())
            {
                if (!string.IsNullOrEmpty(toFindString))
                {
                    var filtered = db.Entries
                        .Include(x => x.Author)
                        .Where(e => e.Topic.Contains(toFindString));
                    if (filtered.Count() > 0)
                        return View(filtered.ToList());
                    return RedirectToAction("EmptyListOfEntries", new { toFindString = toFindString });
                }
                return View(db.Entries.Include(x => x.Author).ToList());
            }
        }

        public ActionResult EmptyListOfEntries(string toFindString)
        {
            ViewBag.toFindString = toFindString;
            using (var db = new BlogContext())
            {
                return View(db.Entries.Include(x => x.Author).ToList());
            }
        }

        public ActionResult DisplaySingle(string toFindString)
        {
            return RedirectToAction("DisplayEntry", "Entry", new { topic = toFindString });
        }
	}
}