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
    public class ItemsController : Controller
    {
        private FMSExpEntities db = new FMSExpEntities();

        // GET: Items
        public async Task<ActionResult> Index()
        {
            var tbl_Items = db.tbl_Items.Include(t => t.tbl_ItemType).Include(t => t.tbl_ItemUnits).Include(t => t.tbl_vendor);
            return View(await tbl_Items.OrderByDescending(q=>q.ID).ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Items tbl_Items = await db.tbl_Items.FindAsync(id);
            if (tbl_Items == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Items);
        }

        // GET: Items/Create
        public ActionResult Create()
        {
            ViewBag.Type = new SelectList(db.tbl_ItemType, "ItemTypeId", "ItemType");
            ViewBag.MeasuringUnit = new SelectList(db.tbl_ItemUnits, "MeasurintUnitID", "MeasuringUnits");
            ViewBag.Supplier = new SelectList(db.tbl_vendor, "ID", "Name");
            ViewBag.ID = Helper.GenericHelper.GetMaxValue("tbl_Items");
            ViewBag.GST = new SelectList(db.tbl_GST, "ID", "Name" );
            ViewBag.CreatedDatetime = DateTime.Now;

            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "ID,Name,HSN_SAC_NO,Type,MeasuringUnit,Manufacture,BarCode,ItemUniqueDescription,Supplier,Photo,UseBatchNo,UseMfgDate,UseExpiryDate,CreatedDatetime")] tbl_Items tbl_Items)
          public async Task<ActionResult> Create([Bind(Include = "ID,Name,HSN_SAC_NO,Type,MeasuringUnit,Manufacture,BarCode,ItemUniqueDescription,Supplier,Photo,UseBatchNo,UseMfgDate,UseExpiryDate,CreatedDatetime,GST")] tbl_Items tbl_Items, [Bind(Include = "OpeningStock")] int? OpeningStock)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Items.Add(tbl_Items);
                await db.SaveChangesAsync();

                tbl_Sequence NewSequenceValue = Helper.GenericHelper.GetNextUpdatedData("tbl_Items");
                db.Entry(NewSequenceValue).State = EntityState.Modified;
                await db.SaveChangesAsync();

                if (OpeningStock > 0)
                {
                    tbl_ItemStock tbl_ItemStock = new tbl_ItemStock()
                    {

                        ID = tbl_Items.ID + "-" + Helper.GenericHelper.GetMaxValue("tbl_ItemStock") + "-1",
                        StockType = "IN",
                        InvoiceNo = "Opening Stock",
                        ItemID = tbl_Items.ID,
                        Qty = OpeningStock,
                        CreatedDatetime = tbl_Items.CreatedDatetime.ToString() ?? DateTime.Now.Date.ToString()

                    };
                    db.tbl_ItemStock.Add(tbl_ItemStock);
                    await db.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }

            ViewBag.Type = new SelectList(db.tbl_ItemType, "ItemTypeId", "ItemType", tbl_Items.Type);
            ViewBag.MeasuringUnit = new SelectList(db.tbl_ItemUnits, "MeasurintUnitID", "MeasuringUnits", tbl_Items.MeasuringUnit);
            ViewBag.Supplier = new SelectList(db.tbl_vendor, "ID", "Name", tbl_Items.Supplier);
            ViewBag.GST = new SelectList(db.tbl_GST, "ID", "GST", tbl_Items.GST);
            return View(tbl_Items);
        }

        // GET: Items/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Items tbl_Items = await db.tbl_Items.FindAsync(id);
            if (tbl_Items == null)
            {
                return HttpNotFound();
            }
            ViewBag.Type = new SelectList(db.tbl_ItemType, "ItemTypeId", "ItemType", tbl_Items.Type);
            ViewBag.MeasuringUnit = new SelectList(db.tbl_ItemUnits, "MeasurintUnitID", "MeasuringUnits", tbl_Items.MeasuringUnit);
            ViewBag.Supplier = new SelectList(db.tbl_vendor, "ID", "Name", tbl_Items.Supplier);
            ViewBag.GST = new SelectList(db.tbl_GST, "ID", "GST", tbl_Items.GST);
            return View(tbl_Items);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "ID,Name,HSN_SAC_NO,Type,MeasuringUnit,Manufacture,BarCode,ItemUniqueDescription,Supplier,Photo,UseBatchNo,UseMfgDate,UseExpiryDate,CreatedDatetime")] tbl_Items tbl_Items)
            public async Task<ActionResult> Edit([Bind(Include = "ID,Name,HSN_SAC_NO,Type,MeasuringUnit,Manufacture,BarCode,ItemUniqueDescription,Supplier,Photo,UseBatchNo,UseMfgDate,UseExpiryDate,CreatedDatetime,GST")] tbl_Items tbl_Items)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Items).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Type = new SelectList(db.tbl_ItemType, "ItemTypeId", "ItemType", tbl_Items.Type);
            ViewBag.MeasuringUnit = new SelectList(db.tbl_ItemUnits, "MeasurintUnitID", "MeasuringUnits", tbl_Items.MeasuringUnit);
            ViewBag.Supplier = new SelectList(db.tbl_vendor, "ID", "Name", tbl_Items.Supplier);
            ViewBag.GST = new SelectList(db.tbl_GST, "ID", "GST", tbl_Items.GST);
            return View(tbl_Items);
        }

        // GET: Items/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Items tbl_Items = await db.tbl_Items.FindAsync(id);
            if (tbl_Items == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Items);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            tbl_Items tbl_Items = await db.tbl_Items.FindAsync(id);
            db.tbl_Items.Remove(tbl_Items);
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
