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
    public class GSTController : Controller
    {
        private FMSExpEntities db = new FMSExpEntities();

        // GET: GST
        public async Task<ActionResult> Index()
        {
            return View(await db.tbl_GST.ToListAsync());
        }

        // GET: GST/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_GST tbl_GST = await db.tbl_GST.FindAsync(id);
            if (tbl_GST == null)
            {
                return HttpNotFound();
            }
            return View(tbl_GST);
        }

        // GET: GST/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GST/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,GST,SGST,CGST,IGST,Comment,Name")] tbl_GST tbl_GST)
        {
            if (ModelState.IsValid)
            {
                db.tbl_GST.Add(tbl_GST);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tbl_GST);
        }

        // GET: GST/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_GST tbl_GST = await db.tbl_GST.FindAsync(id);
            if (tbl_GST == null)
            {
                return HttpNotFound();
            }
            return View(tbl_GST);
        }

        // POST: GST/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,GST,SGST,CGST,IGST,Comment,Name")] tbl_GST tbl_GST)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_GST).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tbl_GST);
        }

        // GET: GST/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_GST tbl_GST = await db.tbl_GST.FindAsync(id);
            if (tbl_GST == null)
            {
                return HttpNotFound();
            }
            return View(tbl_GST);
        }

        // POST: GST/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tbl_GST tbl_GST = await db.tbl_GST.FindAsync(id);
            db.tbl_GST.Remove(tbl_GST);
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
