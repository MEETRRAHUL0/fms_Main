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
    public class MenusController : Controller
    {
        private FMSExpEntities db = new FMSExpEntities();

        // GET: Menus
        public async Task<ActionResult> Index()
        {
            return View(await db.tblMenus.ToListAsync());
        }

        // GET: Menus/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMenu tblMenu = await db.tblMenus.FindAsync(id);
            if (tblMenu == null)
            {
                return HttpNotFound();
            }
            return View(tblMenu);
        }

        // GET: Menus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Menus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,DisplayName,icon,type,ordinal,path,ParentID,IsEnable")] tblMenu tblMenu)
        {
            if (ModelState.IsValid)
            {
                db.tblMenus.Add(tblMenu);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tblMenu);
        }

        // GET: Menus/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMenu tblMenu = await db.tblMenus.FindAsync(id);
            if (tblMenu == null)
            {
                return HttpNotFound();
            }
            return View(tblMenu);
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,DisplayName,icon,type,ordinal,path,ParentID,IsEnable")] tblMenu tblMenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblMenu).State = EntityState.Modified;
                await db.SaveChangesAsync();

                //var childMenus = db.tblMenus.Where(q => q.ParentID == tblMenu.ID);

                //foreach (var childMenu in childMenus)
                //{
                //    childMenu.IsEnable = tblMenu.IsEnable;
                //    db.Entry(childMenu).State = EntityState.Modified;
                //    await db.SaveChangesAsync();
                //}
                return RedirectToAction("Index");
            }
            return View(tblMenu);
        }

        // GET: Menus/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMenu tblMenu = await db.tblMenus.FindAsync(id);
            if (tblMenu == null)
            {
                return HttpNotFound();
            }
            return View(tblMenu);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tblMenu tblMenu = await db.tblMenus.FindAsync(id);
            db.tblMenus.Remove(tblMenu);
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
