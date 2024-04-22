using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;


using PropertyRentalManagementWebSite.Models;

namespace PropertyRentalManagementWebSite.Controllers
{
    public class PropertiesController : Controller
    {
        private PropertyRentalManagementWebSiteEntities db = new PropertyRentalManagementWebSiteEntities();
        // GET: Properties/CreateWithManager
        public ActionResult CreateWithManager()
        {
            // Create a new Property object
            var property = new Property();

            // Optionally, set some default values for the new property
            // property.ManagerID = user.UserID;

            return View(property);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWithManager([Bind(Include = "PropertyID,ManagerID,Address,Name")] Property property)
        {
            if (ModelState.IsValid)
            {
                // Get the logged-in user from the session
                var user = (User)Session["User"];

                // Check if the user type is 'm' and set the ManagerID to the ID of the logged-in user
                if (user != null && user.UserType == "m")
                {
                    property.ManagerID = user.UserID;
                }

                db.Properties.Add(property);
                db.SaveChanges();
                return RedirectToAction("CreateWithManager");
            }

            return View(property);
        }
        public ActionResult ShowProperties(string address)
        {
            var properties = db.Properties.AsQueryable();

            if (!String.IsNullOrEmpty(address))
            {
                address = address.Trim().ToLower(); // Convert address to lowercase
                properties = properties.Where(p => p.Address.ToLower().Contains(address));
            }

            return View(properties.ToList());
        }








        // GET: Properties
        public async Task<ActionResult> Index()
        {
            return View(await db.Properties.ToListAsync());
        }

        // GET: Properties/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Properties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PropertyID,Address,Name,ManagerID")] Property property)
        {
            if (ModelState.IsValid)
            {
                db.Properties.Add(property);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(property);
        }

        // GET: Properties/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = await db.Properties.FindAsync(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        // POST: Properties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PropertyID,Address,Name,ManagerID")] Property property)
        {
            if (ModelState.IsValid)
            {
                db.Entry(property).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(property);
        }

        // GET: Properties/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Property property = await db.Properties.FindAsync(id);
            if (property == null)
            {
                return HttpNotFound();
            }
            return View(property);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Property property = await db.Properties.FindAsync(id);
            db.Properties.Remove(property);
            await db.SaveChangesAsync();
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

