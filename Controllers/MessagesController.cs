using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Property_Rental_Managment_WebSite.Models;



namespace Property_Rental_Managment_WebSite.Controllers
{
    public class MessagesController : Controller
    {
        private PropertyRentalManagementWebSiteEntities db = new PropertyRentalManagementWebSiteEntities();



        // GET: SendMessages
        public ActionResult SendMessages()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Users"); // Redirect to login if user is not signed in
            }

            return View();
        }

        // POST: SendMessages
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendMessages([Bind(Include = "Content,RecipientID")] Message message)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Users"); // Redirect to login if user is not signed in
            }

            if (ModelState.IsValid)
            {
                // Set the SenderID to the current user's ID
                var user = (User)Session["User"];
                message.SenderID = user.UserID;

                db.Messages.Add(message);
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }

                // Redirect to the ShowMessages action in the Messages controller
                return RedirectToAction("ShowMessages", "Messages");
            }

            // If the model is not valid, return to the same view with validation errors
            return View(message);
        }



        public ActionResult ShowMessages()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Users"); // Redirect to login if user is not signed in
            }

            // Get the signed-in user's ID
            var user = (User)Session["User"];

            // Retrieve messages where the recipient's ID matches the signed-in user's ID
            var messages = db.Messages.Where(m => m.RecipientID == user.UserID).ToList();

            return View(messages);
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

        //GET: Messages/Create
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
