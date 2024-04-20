using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Property_Rental_Management.Data;
using Property_Rental_Management.Models;
//using Microsoft.AspNetCore.Mvc;


namespace Property_Rental_Management.Controllers
{
    public class ApartmentsController : Controller
    {
        private Property_Rental_ManagementContext db = new Property_Rental_ManagementContext();

        // GET: Apartments/By the propety ID
        public async Task<ActionResult> ListApartments(int? propertyId)
        {
            if (propertyId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var apartments = await db.Apartments
                                .Where(a => a.PropertyID == propertyId)
                                .Include(a => a.Property)
                                .ToListAsync();

            if (apartments == null || apartments.Count == 0)
            {
                return HttpNotFound();
            }

            return View("Index", apartments);
        }

        //public async Task<IActionResult> Index(string searchString)
        //{
        //    var apartments = from a in _context.Apartments
        //                     select a;

        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        apartments = apartments.Where(a => a.Property.Address.Contains(searchString));
        //    }

        //    return View(await apartments.ToListAsync());
        //}






        // GET: Apartments
        public ActionResult Index(string searchString, int? minRooms, int? maxRooms, decimal? minRent, decimal? maxRent, string status)
        {
            var apartments = db.Apartments.Include(a => a.Property);

            if (!String.IsNullOrEmpty(searchString))
            {
                apartments = apartments.Where(a =>
                    a.Property.Address.Contains(searchString) ||
                    a.NumberOfRooms.ToString().Contains(searchString) ||
                    a.Rent.ToString().Contains(searchString) ||
                    a.Status.Contains(searchString)
                );
            }

            if (minRooms.HasValue)
            {
                apartments = apartments.Where(a => a.NumberOfRooms >= minRooms);
            }

            if (maxRooms.HasValue)
            {
                apartments = apartments.Where(a => a.NumberOfRooms <= maxRooms);
            }

            if (minRent.HasValue)
            {
                apartments = apartments.Where(a => a.Rent >= minRent);
            }

            if (maxRent.HasValue)
            {
                apartments = apartments.Where(a => a.Rent <= maxRent);
            }

            if (!String.IsNullOrEmpty(status))
            {
                apartments = apartments.Where(a => a.Status == status);
            }

            return View(apartments.ToList());
        }



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
            ViewBag.PropertyID = new SelectList(db.Properties, "PropertyID", "Address");
            return View();
        }

        // POST: Apartments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

            ViewBag.PropertyID = new SelectList(db.Properties, "PropertyID", "Address", apartment.PropertyID);
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
