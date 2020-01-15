using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class EntriesListController : Controller
    {
        private Task<Account> GetCurrentUser(BlogContext db)
        {
            return db.Accounts.SingleAsync(a => a.Name.Equals(User.Identity.Name));
        }

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
                    return RedirectToAction("EmptyListOfEntries", new { toFindString });
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

        [HttpGet]
        public ActionResult CommentedEntries()
        {
            if (User.Identity.IsAuthenticated)
            {
                using (var db = new BlogContext())
                {
                    var user = GetCurrentUser(db);
                    var latestCommentedEntries = db
                        .Entries
                        .Include(x => x.Comments.Select(c => c.Author))
                        .OrderBy(e => e.Comments.Where(c => c.Author.Name.Equals(User.Identity.Name)).Max(c => c.Date))
                        .Take(3);
                    return View(latestCommentedEntries);
                }
            }
            else
                return new EmptyResult();
        }

        [HttpGet]
        public ActionResult SuggestedEntries()
        {
            if (User.Identity.IsAuthenticated)
            {
                using (var db = new BlogContext())
                {
                    var user = GetCurrentUser(db);
                    var mostPrefferedByUser = db
                        .TagsToUsers
                        .Where(a => a.Account.Name.Equals(User.Identity.Name))
                        .OrderBy(ttu => ttu.TimesSeen)
                        .Take(3)
                        .Select(ttu => ttu.Tag.TagId);
                    var random = new Random((int)DateTime.Now.ToBinary());
                    var randomEntriesContaining = db
                        .Entries
                        .Where(e => e.Tags.Count(t => mostPrefferedByUser.Contains(t.TagId)) > 0)
                        .Include(e => e.Author)
                        .Include(e => e.Comments)
                        .OrderBy(e => random.Next())
                        .ToList();
                    return View(randomEntriesContaining);
                }
            }
            else
            {
                return new EmptyResult();
            }
        }
	}
}