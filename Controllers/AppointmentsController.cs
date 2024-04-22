using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Property_Rental_Managment_WebSite.Models;

namespace PropertyRentalManagementWebSite.Controllers
{
    public class AppointmentsController : Controller
    {
        private PropertyRentalManagementWebSiteEntities db = new PropertyRentalManagementWebSiteEntities();



        public ActionResult ScheduleAppointment()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Users");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ScheduleAppointment([Bind(Include = "RecipientID, TimeAndDate")] Appointment appointment)
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Users");
            }

            if (ModelState.IsValid)
            {
                var user = (User)Session["User"];
                appointment.SenderID = user.UserID;
                appointment.Confirmed = false;

                db.Appointments.Add(appointment);
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }

                return RedirectToAction("ShowRequestedAppointment", "Appointments");
            }

            return View(appointment);
        }


        public ActionResult ShowConfirmedAppointment()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Users");
            }

            var user = (User)Session["User"];

            var appointments = db.Appointments.Where(m => (m.SenderID == user.UserID || m.RecipientID == user.UserID) && m.Confirmed == true).ToList();

            return View(appointments);
        }

        public ActionResult ShowRequestedAppointment()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "Users");
            }

            var user = (User)Session["User"];

            var appointments = db.Appointments.Where(m => (m.RecipientID == user.UserID) && m.Confirmed == false).ToList();

            return View(appointments);
        }

        [HttpPost]
        public async Task<ActionResult> ConfirmAppointment(int id)
        {
            var appointment = await db.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            appointment.Confirmed = true;
            db.Entry(appointment).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("ShowRequestedAppointment");
        }










        // GET: Appointments
        public async Task<ActionResult> Index()
        {
            return View(await db.Appointments.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = await db.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: Appointments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AppointmentID,SenderID,RecipientID,TimeAndDate,Confirmed")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Appointments.Add(appointment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = await db.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AppointmentID,SenderID,RecipientID,TimeAndDate,Confirmed")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = await db.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Appointment appointment = await db.Appointments.FindAsync(id);
            db.Appointments.Remove(appointment);
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
