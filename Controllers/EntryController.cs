using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Drawing;
using System.IO;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class EntryController : Controller
    {
        private Task<Account> GetCurrentUser(BlogContext db)
        {
            return db.Accounts.SingleAsync(a => a.Name.Equals(User.Identity.Name));
        }

        public ActionResult ReadImage(int entryId)
        {
            using (var db = new BlogContext())
            {
                var e = db.Entries.Find(entryId);
                if (e.Image != null)
                    return File(e.Image, "image.png");
                return File(new byte[1], "empty.png");
            }
        }

        private void IncrementUsersTags(BlogEntry entry)
        {
            foreach (var tag in entry.Tags)
            {
                tag.TagToUsers
                    .Where(ttu => ttu.Account.Name.Equals(User.Identity.Name))
                    .ToList()
                    .ForEach(ttu => ttu.TimesSeen++);
            }
        }

        private void AddMissingTagToUsers(BlogContext db, BlogEntry entry)
        {
            var user = GetCurrentUser(db);
            var missing = entry
                .Tags
                .Where(t => user.Result
                    .TagsToUser.Count(ttu => ttu.Tag.Name.Equals(t.Name)) == 0)
                .Select(tag => new TagToUser
                {
                    TimesSeen = 0,
                    Account = user.Result,
                    Tag = tag
                }).ToList();
            db.TagsToUsers.AddRange(missing);
        }

        //
        // GET: /Entry/
        [HttpGet]
        public ActionResult DisplayEntry(string topic)
        {
            using (var db = new BlogContext())
            {
                var entry = db.Entries
                    .Include(e => e.Author)
                    .Include(e => e.Comments.Select(c => c.Author))
                    .Include(e => e.Tags.Select(t => t.TagToUsers.Select(ttu => ttu.Account)))
                    .First(e => e.Topic.Equals(topic));
                entry.Seen++;
                if (User.Identity.IsAuthenticated)
                {
                    AddMissingTagToUsers(db, entry);
                    IncrementUsersTags(entry);
                }
                db.SaveChanges();
                return View(entry);
            }
        }

        public ActionResult PreviousPage(DateTime currentTopic)
        {
            using (var db = new BlogContext())
            {
                var pre = db.Entries.LastOrDefault(e => e.Date < currentTopic);
                if (pre == null)
                {
                    return RedirectToAction("DisplayEntry", new { topic = currentTopic });
                }
                return RedirectToAction("DisplayEntry", new { topic = pre.Topic });
            }
        }

        public ActionResult NextPage(DateTime currentTopic)
        {
            using (var db = new Blog.Models.BlogContext())
            {
                var post = db.Entries.First(e => e.Date > currentTopic);
                if (post == null)
                {
                    return RedirectToAction("DisplayEntry", new { topic = currentTopic });
                }
                return RedirectToAction("DisplayEntry", new { topic = post.Topic });
            } 
        }

        [HttpGet]
        [Authorize(Roles = UserTypes.poster + "," + UserTypes.admin)]
        public ActionResult NewEntry()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = UserTypes.poster + "," + UserTypes.admin)]
        public ActionResult NewEntry(BlogEntry entry, string collectedTags)
        {
            if (ModelState.IsValid)
            {
                using (var db = new BlogContext())
                {
                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];
                        if (file != null)
                        {
                            var fileStream = file.InputStream;
                            byte[] data = new byte[fileStream.Length];
                            fileStream.Read(data, 0, (int)fileStream.Length);
                            entry.Image = data;
                        }
                    }
                    if (!string.IsNullOrEmpty(collectedTags))
                    {
                        var tagArray = collectedTags.Split(' ').Distinct();
                        foreach (var tag in tagArray)
                        {
                            if (!db.Tags.Any(t => t.Name.Equals(tag)))
                            {
                                var tagStruct = new EntryTag
                                {
                                    Name = tag
                                };
                                db.Tags.Add(tagStruct);
                            }
                        }
                        db.SaveChanges(); // save tags, because entry won't get linked to tags if they are not in db
                        entry.Tags = db.Tags.Where(t => tagArray.Contains(t.Name)).ToList();
                    }
                    entry.Date = DateTime.Now;
                    entry.Author = db.Accounts.First(a => a.Name.Equals(User.Identity.Name));
                    db.Entries.Add(entry);
                    db.SaveChanges();
                    return RedirectToAction("ListOfEntries", "EntriesList");
                }
            }
            else
            {
                return View(entry);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult NewComment(int entryId)
        {
            using (var db = new BlogContext())
            {
                return View(new Comment { Entry = db.Entries.Find(entryId) });
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult NewComment(Comment comment, int entryId)
        {
            if (ModelState.IsValid)
            {
                using (var db = new BlogContext())
                {
                    comment.Date = System.DateTime.Now;
                    comment.Entry = db.Entries.Single(s => s.EntryId == entryId);
                    comment.Author = db.Accounts.Single(a => a.Name.Equals(User.Identity.Name));
                    db.Comments.Add(comment);
                    db.SaveChanges();
                    return RedirectToAction("DisplayEntry", new { topic = comment.Entry.Topic });
                }
            }
            else
            {
                return View(new Comment { Text = comment.Text, Date = comment.Date, 
                    Entry = new BlogContext().Entries.Find(entryId) });
            }
        }
	}
}