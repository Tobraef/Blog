using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Web.Security;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAccount(Account acc)
        {
            if (ModelState.IsValid)
            {
                using (var db = new BlogContext())
                {
                    db.Accounts.Add(acc);
                    db.SaveChanges();
                    return View("AccountCreated");
                }
            }
            else
            {
                return View(acc);
            }
        }

        [HttpGet]
        public ActionResult EditAccount(string accountName)
        {
            using (var db = new BlogContext())
            {
                return View(db.Accounts.First(a => a.Name.Equals(accountName)));
            }
        }

        [HttpPost]
        public ActionResult EditAccount(Account acc, string name)
        {
            using (var db = new BlogContext())
            {
                db.Accounts.Remove(db.Accounts.First(a => a.Name.Equals(name)));
                db.Accounts.Add(acc);
                return View("AccountUpdated");
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Account acc, string returnUrl /* form: /Controller/View */)
        {
            if (ModelState.IsValid)
            {
                using (var db = new BlogContext())
                {
                    if (db.Accounts.Count(a => a.Name.Equals(acc.Name) && a.Password.Equals(acc.Password)) == 1)
                    {
                        FormsAuthentication.SetAuthCookie(acc.Name, false);
                        if (string.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("StartView", "StartPage");
                        else
                        {
                            var parts = returnUrl.Split('/');
                            return RedirectToAction(parts[2], parts[1]);
                        }
                    }
                }
            }
            return View(acc);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("StartView", "StartPage");
        }

        [HttpGet]
        [Authorize(Roles=UserTypes.admin)]
        public ActionResult AdminTools()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles=UserTypes.admin)]
        public ActionResult CreateAccountAdmin()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles=UserTypes.admin)]
        public ActionResult CreateAccountAdmin(Account acc)
        {
            if (ModelState.IsValid)
            using (var db = new BlogContext())
            {
                db.Accounts.Add(acc);
                db.SaveChanges();
                return View("AccountCreated");
            }
            else
            {
                return View(acc);
            }
        }
    }
}