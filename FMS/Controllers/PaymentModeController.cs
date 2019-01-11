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
    public class PaymentModeController : Controller
    {
        private FMSExpEntities db = new FMSExpEntities();

        // GET: PaymentMode
        public async Task<ActionResult> Index()
        {
            return View(await db.tbl_PaymentMode.ToListAsync());
        }

        // GET: PaymentMode/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PaymentMode tbl_PaymentMode = await db.tbl_PaymentMode.FindAsync(id);
            if (tbl_PaymentMode == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PaymentMode);
        }

        // GET: PaymentMode/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentMode/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Mode,Type,IsVisible")] tbl_PaymentMode tbl_PaymentMode)
        {
            if (ModelState.IsValid)
            {
                db.tbl_PaymentMode.Add(tbl_PaymentMode);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tbl_PaymentMode);
        }

        // GET: PaymentMode/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PaymentMode tbl_PaymentMode = await db.tbl_PaymentMode.FindAsync(id);
            if (tbl_PaymentMode == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PaymentMode);
        }

        // POST: PaymentMode/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Mode,Type,IsVisible")] tbl_PaymentMode tbl_PaymentMode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_PaymentMode).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tbl_PaymentMode);
        }

        // GET: PaymentMode/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_PaymentMode tbl_PaymentMode = await db.tbl_PaymentMode.FindAsync(id);
            if (tbl_PaymentMode == null)
            {
                return HttpNotFound();
            }
            return View(tbl_PaymentMode);
        }

        // POST: PaymentMode/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tbl_PaymentMode tbl_PaymentMode = await db.tbl_PaymentMode.FindAsync(id);
            db.tbl_PaymentMode.Remove(tbl_PaymentMode);
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
