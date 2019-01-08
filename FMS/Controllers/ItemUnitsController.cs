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
    public class ItemUnitsController : Controller
    {
        private FMSExpEntities db = new FMSExpEntities();

        // GET: ItemUnits
        public async Task<ActionResult> Index()
        {
            return View(await db.tbl_ItemUnits.ToListAsync());
        }

        // GET: ItemUnits/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_ItemUnits tbl_ItemUnits = await db.tbl_ItemUnits.FindAsync(id);
            if (tbl_ItemUnits == null)
            {
                return HttpNotFound();
            }
            return View(tbl_ItemUnits);
        }

        // GET: ItemUnits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ItemUnits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MeasurintUnitID,MeasuringUnits")] tbl_ItemUnits tbl_ItemUnits)
        {
            if (ModelState.IsValid)
            {
                db.tbl_ItemUnits.Add(tbl_ItemUnits);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tbl_ItemUnits);
        }

        // GET: ItemUnits/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_ItemUnits tbl_ItemUnits = await db.tbl_ItemUnits.FindAsync(id);
            if (tbl_ItemUnits == null)
            {
                return HttpNotFound();
            }
            return View(tbl_ItemUnits);
        }

        // POST: ItemUnits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MeasurintUnitID,MeasuringUnits")] tbl_ItemUnits tbl_ItemUnits)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_ItemUnits).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tbl_ItemUnits);
        }

        // GET: ItemUnits/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_ItemUnits tbl_ItemUnits = await db.tbl_ItemUnits.FindAsync(id);
            if (tbl_ItemUnits == null)
            {
                return HttpNotFound();
            }
            return View(tbl_ItemUnits);
        }

        // POST: ItemUnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tbl_ItemUnits tbl_ItemUnits = await db.tbl_ItemUnits.FindAsync(id);
            db.tbl_ItemUnits.Remove(tbl_ItemUnits);
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
