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


namespace Property_Rental_Managment_WebSite.Controllers
{
    public class ApartmentsController : Controller
    {
        private PropertyRentalManagementWebSiteEntities db = new PropertyRentalManagementWebSiteEntities();

        public async Task<ActionResult> ShowApartments(int? propertyId, int? numberOfRooms, decimal? rent, string status)
        {
            var apartments = db.Apartments.AsQueryable();

            if (propertyId.HasValue)
            {
                apartments = apartments.Where(a => a.PropertyID == propertyId.Value);
            }

            if (numberOfRooms.HasValue)
            {
                apartments = apartments.Where(a => a.NumberOfRooms == numberOfRooms.Value);
            }

            if (rent.HasValue)
            {
                apartments = apartments.Where(a => a.Rent <= rent.Value);
            }

            if (!String.IsNullOrEmpty(status))
            {
                apartments = apartments.Where(a => a.Status.Contains(status));
            }

            return View("ShowApartments", await apartments.ToListAsync());
        }








        // GET: Apartments
        public ActionResult Index()
        {
            var apartments = db.Apartments.ToList();
            return View(apartments);
        }

        // GET: Apartments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            return View(apartment);
        }

        // GET: Apartments/Create
        public ActionResult Create()
        {
            ViewBag.Properties = new SelectList(db.Properties, "PropertyID", "Address");
            return View();
        }




        // POST: Apartments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ApartmentID,NumberOfRooms,Rent,Status,PropertyID")] Apartment apartment)
        {
            if (ModelState.IsValid)
            {
                db.Apartments.Add(apartment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Properties = new SelectList(db.Properties, "PropertyID", "Address");

            return View(apartment);
        }

        // GET: Apartments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            return View(apartment);
        }

        // POST: Apartments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApartmentID,NumberOfRooms,Rent,Status,PropertyID")] Apartment apartment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(apartment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(apartment);
        }

        // GET: Apartments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            return View(apartment);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Apartment apartment = db.Apartments.Find(id);
            db.Apartments.Remove(apartment);
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

