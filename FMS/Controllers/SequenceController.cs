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
    public class SequenceController : Controller
    {
        private FMSExpEntities db = new FMSExpEntities();

        // GET: Sequence
        public async Task<ActionResult> Index()
        {
            return View(await db.tbl_Sequence.ToListAsync());
        }

        // GET: Sequence/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Sequence tbl_Sequence = await db.tbl_Sequence.FindAsync(id);
            if (tbl_Sequence == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Sequence);
        }

        // GET: Sequence/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sequence/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,TableName,PreFix,RecordCount")] tbl_Sequence tbl_Sequence)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Sequence.Add(tbl_Sequence);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tbl_Sequence);
        }

        // GET: Sequence/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Sequence tbl_Sequence = await db.tbl_Sequence.FindAsync(id);
            if (tbl_Sequence == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Sequence);
        }

        // POST: Sequence/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,TableName,PreFix,RecordCount")] tbl_Sequence tbl_Sequence)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Sequence).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tbl_Sequence);
        }

        // GET: Sequence/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Sequence tbl_Sequence = await db.tbl_Sequence.FindAsync(id);
            if (tbl_Sequence == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Sequence);
        }

        // POST: Sequence/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tbl_Sequence tbl_Sequence = await db.tbl_Sequence.FindAsync(id);
            db.tbl_Sequence.Remove(tbl_Sequence);
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
