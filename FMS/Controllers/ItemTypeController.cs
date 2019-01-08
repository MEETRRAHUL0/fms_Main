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
    public class ItemTypeController : Controller
    {
        private FMSExpEntities db = new FMSExpEntities();

        // GET: tbl_ItemType
        public async Task<ActionResult> Index()
        {
            return View(await db.tbl_ItemType.ToListAsync());
        }

        // GET: tbl_ItemType/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_ItemType tbl_ItemType = await db.tbl_ItemType.FindAsync(id);
            if (tbl_ItemType == null)
            {
                return HttpNotFound();
            }
            return View(tbl_ItemType);
        }

        // GET: tbl_ItemType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tbl_ItemType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ItemTypeId,ItemType")] tbl_ItemType tbl_ItemType)
        {
            if (ModelState.IsValid)
            {
                db.tbl_ItemType.Add(tbl_ItemType);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tbl_ItemType);
        }

        // GET: tbl_ItemType/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_ItemType tbl_ItemType = await db.tbl_ItemType.FindAsync(id);
            if (tbl_ItemType == null)
            {
                return HttpNotFound();
            }
            return View(tbl_ItemType);
        }

        // POST: tbl_ItemType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ItemTypeId,ItemType")] tbl_ItemType tbl_ItemType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_ItemType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tbl_ItemType);
        }

        // GET: tbl_ItemType/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_ItemType tbl_ItemType = await db.tbl_ItemType.FindAsync(id);
            if (tbl_ItemType == null)
            {
                return HttpNotFound();
            }
            return View(tbl_ItemType);
        }

        // POST: tbl_ItemType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tbl_ItemType tbl_ItemType = await db.tbl_ItemType.FindAsync(id);
            db.tbl_ItemType.Remove(tbl_ItemType);
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
