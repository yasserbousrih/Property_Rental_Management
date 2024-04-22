using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Property_Rental_Managment_WebSite.Models;


namespace Property_Rental_Managment_WebSite.Controllers
{
    public class UsersController : Controller
    {
        private PropertyRentalManagementWebSiteEntities db = new PropertyRentalManagementWebSiteEntities();





        public ActionResult Logout()
        {
            Session["User"] = null;
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }




        public ActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            var matchingUsers = db.Users.Where(u => u.Email == user.Email && u.Password == user.Password).ToList();
            if (matchingUsers.Count > 1)
            {
                // Handle the error: multiple users with the same email and password found
                ModelState.AddModelError("", "Multiple users with the same credentials found. Please contact support.");
            }
            else if (matchingUsers.Count == 1)
            {
                // User found in the database
                // You can set the user in the session here
                Session["User"] = matchingUsers.First();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // User not found
                ModelState.AddModelError("", "Invalid login credentials.");
            }
            return View();
        }



        // GET: Users/Signup
        public ActionResult Signup()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup([Bind(Include = "Name,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                // Check if a user with the same email already exists
                var existingUser = db.Users.FirstOrDefault(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    // If a user with the same email already exists, add a model error
                    ModelState.AddModelError("Email", "A user with this email already exists.");
                    return View(user);
                }

                user.UserType = "p"; // Set UserType to 'p'
                db.Users.Add(user);
                db.SaveChanges();
                Session["User"] = user;
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }










        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,Name,Email,Password,UserType")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,Name,Email,Password,UserType")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}