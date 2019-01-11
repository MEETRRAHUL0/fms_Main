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
    public class vendorTypeController : Controller
    {
        private FMSExpEntities db = new FMSExpEntities();

        // GET: vendorType
        public async Task<ActionResult> Index()
        {
            return View(await db.tbl_vendorType.ToListAsync());
        }

        // GET: vendorType/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_vendorType tbl_vendorType = await db.tbl_vendorType.FindAsync(id);
            if (tbl_vendorType == null)
            {
                return HttpNotFound();
            }
            return View(tbl_vendorType);
        }

        // GET: vendorType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: vendorType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AutoID,ID,Name")] tbl_vendorType tbl_vendorType)
        {
            if (ModelState.IsValid)
            {
                db.tbl_vendorType.Add(tbl_vendorType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tbl_vendorType);
        }

        // GET: vendorType/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_vendorType tbl_vendorType = await db.tbl_vendorType.FindAsync(id);
            if (tbl_vendorType == null)
            {
                return HttpNotFound();
            }
            return View(tbl_vendorType);
        }

        // POST: vendorType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AutoID,ID,Name")] tbl_vendorType tbl_vendorType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_vendorType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tbl_vendorType);
        }

        // GET: vendorType/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_vendorType tbl_vendorType = await db.tbl_vendorType.FindAsync(id);
            if (tbl_vendorType == null)
            {
                return HttpNotFound();
            }
            return View(tbl_vendorType);
        }

        // POST: vendorType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            tbl_vendorType tbl_vendorType = await db.tbl_vendorType.FindAsync(id);
            db.tbl_vendorType.Remove(tbl_vendorType);
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
