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
    public class StartPageController : Controller
    {
        public ActionResult GetEntryImage(int id)
        {
            using (var db = new BlogContext())
            {
                return File(db.Entries.Find(id).Image, "image.png");
            }
        }

        public ActionResult DisplayEntry(int id)
        {
            using (var db = new BlogContext())
            return RedirectToAction("DisplayEntry", "Entry", new { topic = db.Entries.Find(id).Topic });
        }

        private IEnumerable<int> GetRandomIndexes(int count, int max)
        {
            max++;
            var r = new Random((int)DateTime.Now.ToBinary());
            var nums = new List<int>();
            for (int i = 0; i < count; ++i)
            {
                int rN = r.Next(max);
                while (nums.Contains(rN))
                {
                    rN = r.Next(max);
                }
                nums.Add(rN);
            }
            return nums;
        }

        private IEnumerable<EntryPreview> GetRandomPreviews(BlogContext db, int count)
        {
            int max = db.Entries.Count();
            if (count > max)
            {
                return db.Entries.Select(e => new EntryPreview
                {
                    EntryId = e.EntryId,
                    Title = e.Topic
                });
            }
            var loading = db.Entries.LoadAsync();
            var toRet = new EntryPreview[count];
            var indexes = GetRandomIndexes(count, max);
            loading.Wait();
            var tasks = new Task[count];
            for (int i = 0; i < count; ++i)
            {
                tasks[i] = Task.Factory.StartNew(idx =>
                {
                    var e = db.Entries.Local.ElementAt(indexes.ElementAt((int)idx));
                    toRet[(int)idx] = new EntryPreview { EntryId = e.EntryId, Title = e.Topic };
                }, i);
            }
            Task.WaitAll(tasks);
            return toRet;
        }


        [HttpGet]
        public ActionResult StartView()
        {
            using (var db = new BlogContext())
            {
                return View(GetRandomPreviews(db, 4).ToList());
            }
        }

        [HttpGet]
        public ActionResult UploadImage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            if (file != null)
            {
                using (var db = new Blog.Models.BlogContext())
                {
                    var stream = file.InputStream;
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);
                    db.Entries.Single().Image = bytes;
                }
            }
            else
            {
                throw new Exception("f...");
            }
            return RedirectToAction("StartView");
        }
	}
}