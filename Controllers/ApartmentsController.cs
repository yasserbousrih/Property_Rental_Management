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

        // GET: Apartments
        public async Task<ActionResult> Index()
        {
            var apartments = db.Apartments.Include(a => a.Property);
            return View(await apartments.ToListAsync());
        }
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





        //// GET: Apartments/By the propety ID
        //public async Task<ActionResult> ListApartments(int? propertyId)
        //{
        //    if (propertyId == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    var apartments = await db.Apartments
        //                        .Where(a => a.PropertyID == propertyId)
        //                        .Include(a => a.Property)
        //                        .ToListAsync();

        //    if (apartments == null || apartments.Count == 0)
        //    {
        //        return HttpNotFound();
        //    }

        //    return View("Index", apartments);
        //}


        // GET: Apartments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = await db.Apartments.FindAsync(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            return View(apartment);
        }

        // GET: Apartments/Create
        public ActionResult Create()
        {
            // Use ViewBag to pass the list of Properties to the view
            ViewBag.PropertyID = new SelectList(db.Properties, "PropertyID", "Name");
            return View();
        }

        // POST: Apartments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ApartmentID,NumberOfRooms,Rent,Status,PropertyID")] Apartment apartment)
        {
            if (ModelState.IsValid)
            {
                db.Apartments.Add(apartment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // If the model state is not valid, repopulate the PropertyID dropdown list
            ViewBag.PropertyID = new SelectList(db.Properties, "PropertyID", "Name", apartment.PropertyID);
            return View(apartment);
        }


        // GET: Apartments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = await db.Apartments.FindAsync(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PropertyID = new SelectList(db.Properties, "PropertyID", "Address", apartment.PropertyID);
            return View(apartment);
        }

        // POST: Apartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ApartmentID,NumberOfRooms,Rent,Status,PropertyID")] Apartment apartment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(apartment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PropertyID = new SelectList(db.Properties, "PropertyID", "Address", apartment.PropertyID);
            return View(apartment);
        }

        // GET: Apartments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = await db.Apartments.FindAsync(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            return View(apartment);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Apartment apartment = await db.Apartments.FindAsync(id);
            db.Apartments.Remove(apartment);
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
