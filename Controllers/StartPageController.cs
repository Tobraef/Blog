using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class StartPageController : Controller
    {
        //
        // GET: /StartPage/
        public ActionResult StartView()
        {
            return View();
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