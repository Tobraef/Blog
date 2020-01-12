﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Drawing;
using System.IO;
using System.Data.Entity;

namespace Blog.Controllers
{
    public class EntryController : Controller
    {
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

        //
        // GET: /Entry/
        public ActionResult DisplayEntry(string topic)
        {
            using (var db = new Blog.Models.BlogContext())
            {
                var entry = db.Entries
                    .Include(e => e.Author)
                    .Include(e => e.Comments.Select(c => c.Author))
                    .Where(e => e.Topic.Equals(topic))
                    .Single();
                return View(entry);
            }
        }

        public ActionResult PreviousPage(DateTime currentTopic)
        {
            using (var db = new Blog.Models.BlogContext())
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
        public ActionResult NewEntry(BlogEntry entry, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            using (var db = new BlogContext())
            {
                if (file != null)
                {
                    var fileStream = file.InputStream;
                    byte[] data = new byte[fileStream.Length];
                    fileStream.Read(data, 0, (int)fileStream.Length);
                    entry.Image = data;
                }
                else
                {
                    throw new Exception("Shiet");
                }
                entry.Date = DateTime.Now;
                entry.Author = db.Accounts.First(a => a.Name.Equals(User.Identity.Name));
                db.Entries.Add(entry);
                db.SaveChanges();
                return RedirectToAction("ListOfEntries", "EntriesList");
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