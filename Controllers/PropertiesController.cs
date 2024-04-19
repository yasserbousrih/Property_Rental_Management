using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Property_Rental_Management.Data;
using Property_Rental_Management.Models;

namespace Property_Rental_Management.Controllers
{
    public class PropertiesController : Controller
    {
        private Property_Rental_ManagementContext db = new Property_Rental_ManagementContext();











        // GET: Properties
        public ActionResult Index()
        {
            return View(db.Properties.ToList());
        }

        // GET: Properties/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = db.Properties.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        // GET: Properties/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Properties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PropertyID,NumberOfRooms,Rent,Status,ManagerID")] Property property)
        {
            // Check if the user is a manager
            if (UserIsManager())
            {
                // Get the manager's UserID and set it as the ManagerID of the property
                var managerId = GetManagerId();
                property.ManagerID = managerId;
            }

            if (ModelState.IsValid)
            {
                // Save the property
                db.Properties.Add(property);
                db.SaveChanges();
                return RedirectToAction("Index", "Properties"); // Redirect to the Index action of the Properties controller
            }


            // If the model state is not valid, return the view with validation errors
            return View(property);
        }
        // Helper method to check if the user is a manager
        private bool UserIsManager()
        {
            var user = (Property_Rental_Management.Models.User)Session["User"];
            return user != null && user.UserType == "m";
        }

        // Helper method to get the manager's UserID
        private int GetManagerId()
        {
            var user = (Property_Rental_Management.Models.User)Session["User"];
            return user != null ? user.UserID : 0;
        }



        // GET: Properties/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = db.Properties.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        // POST: Properties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PropertyID,NumberOfRooms,Rent,Status,ManagerID")] Property property)
        {
            if (ModelState.IsValid)
            {
                db.Entry(property).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(property);
        }

        // GET: Properties/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = db.Properties.Find(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Property property = db.Properties.Find(id);
            db.Properties.Remove(property);
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
