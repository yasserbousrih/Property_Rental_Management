using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Property_Rental_Management.Data;
using Property_Rental_Management.Models;

namespace Property_Rental_Management.Controllers
{
    public class MessagesController : Controller
    {
        private Property_Rental_ManagementContext db = new Property_Rental_ManagementContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task SendMessage([Bind(Include = "Content,RecipientID")] Message message)
        {
            if (ModelState.IsValid)
            {
                // Set the SenderID to the current user's ID
                var user = (User)Session["User"];
                if (user != null)
                {
                    message.SenderID = user.UserID;
                }

                db.Messages.Add(message);
                await db.SaveChangesAsync();

                // Set a message to be displayed
                TempData["Message"] = "Message sent successfully!";
                // No return statement
            }
        }




        public ActionResult Create()
        {
            // Retrieve the list of users from the database
            var users = db.Users.ToList();

            // Map the list of users to SelectListItems
            var userListItems = users.Select(u => new SelectListItem
            {
                Value = u.UserID.ToString(),
                Text = u.Name // Adjust this to the property of User that represents the user's name
            });

            // Pass the list of users as SelectListItems to the view
            ViewBag.Users = userListItems;

            return View();
        }












        public ActionResult SendMessage()
        {
            // Get users from the database
            var users = db.Users.ToList();
            // Create a SelectList for dropdown
            ViewBag.Users = new SelectList(users, "UserID", "Username");
            // Create a new message model
            var message = new Message();
            return PartialView("_SendMessage", message);
        }




        // GET: Messages
        public ActionResult Index()
        {
            return View(db.Messages.ToList());
        }

        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: Messages/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageID,Content,SenderID,RecipientID")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(message);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageID,Content,SenderID,RecipientID")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
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
