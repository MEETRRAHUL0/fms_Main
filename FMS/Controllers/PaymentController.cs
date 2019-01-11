using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FMS;

namespace FMS.Controllers
{
    public class PaymentController : Controller
    {
        private FMSExpEntities db = new FMSExpEntities();

        // GET: Payment
        public async Task<ActionResult> Index()
        {
            var tbl_Payment = db.tbl_Payment.Include(t => t.tbl_vendor).Include(t => t.tbl_Transaction);
            return View(await tbl_Payment.ToListAsync());
        }

        // GET: Payment/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Payment tbl_Payment = await db.tbl_Payment.FindAsync(id);
            if (tbl_Payment == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Payment);
        }

        // GET: Payment/Create
        public ActionResult Create()
        {
            ViewBag.PartyName = new SelectList(db.tbl_vendor, "ID", "Name");

            return View();
        }

        // POST: Payment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AutoID,ID,PaymentDate,PartyName,Amount,PaymentMode,Remark,CreatedDatetime")] tbl_Payment tbl_Payment)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Payment.Add(tbl_Payment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.PartyName = new SelectList(db.tbl_vendor, "ID", "Name", tbl_Payment.PartyName);

            return View(tbl_Payment);
        }

        // GET: Payment/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Payment tbl_Payment = await db.tbl_Payment.FindAsync(id);
            if (tbl_Payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.PartyName = new SelectList(db.tbl_vendor, "ID", "Name", tbl_Payment.PartyName);

            return View(tbl_Payment);
        }

        // POST: Payment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AutoID,ID,PaymentDate,PartyName,Amount,PaymentMode,Remark,CreatedDatetime")] tbl_Payment tbl_Payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Payment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PartyName = new SelectList(db.tbl_vendor, "ID", "Name", tbl_Payment.PartyName);

            return View(tbl_Payment);
        }

        // GET: Payment/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Payment tbl_Payment = await db.tbl_Payment.FindAsync(id);
            if (tbl_Payment == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Payment);
        }

        // POST: Payment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            tbl_Payment tbl_Payment = await db.tbl_Payment.FindAsync(id);
            db.tbl_Payment.Remove(tbl_Payment);
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
