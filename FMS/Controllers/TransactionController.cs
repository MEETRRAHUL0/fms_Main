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
    public class TransactionController : Controller
    {
        private FMSExpEntities db = new FMSExpEntities();

        // GET: Transaction
        public async Task<ActionResult> Index()
        {
            var tbl_Transaction = db.tbl_Transaction.Include(t => t.tbl_Purchase).Include(t => t.tbl_Sale).Include(t => t.tbl_vendor);
            return View(await tbl_Transaction.ToListAsync());
        }

        // GET: Transaction/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Transaction tbl_Transaction = await db.tbl_Transaction.FindAsync(id);
            if (tbl_Transaction == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Transaction);
        }

        // GET: Transaction/Create
        public ActionResult Create()
        {
            ViewBag.PurchaseID = new SelectList(db.tbl_Purchase, "ID", "PartyInvoiceNo");
            ViewBag.SalesID = new SelectList(db.tbl_Sale, "SaleID", "InvoiceID");
            ViewBag.VendorID = new SelectList(db.tbl_vendor, "ID", "Name");
            ViewBag.PaymentID = new SelectList(db.tbl_Payment, "ID", "PaymentDate");
            return View();
        }

        // POST: Transaction/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AutoID,ID,VendorID,PurchaseID,SalesID,EntryID,EntryDate,EntryType,Amount,Status,CreatedDatetime,TransactionType,TransactionRef,PaymentID")] tbl_Transaction tbl_Transaction)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Transaction.Add(tbl_Transaction);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.PurchaseID = new SelectList(db.tbl_Purchase, "ID", "PartyInvoiceNo", tbl_Transaction.PurchaseID);
            ViewBag.SalesID = new SelectList(db.tbl_Sale, "SaleID", "InvoiceID", tbl_Transaction.SalesID);
            ViewBag.VendorID = new SelectList(db.tbl_vendor, "ID", "Name", tbl_Transaction.VendorID);
            ViewBag.PaymentID = new SelectList(db.tbl_Payment, "ID", "PaymentDate", tbl_Transaction.PaymentID);
            return View(tbl_Transaction);
        }

        // GET: Transaction/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Transaction tbl_Transaction = await db.tbl_Transaction.FindAsync(id);
            if (tbl_Transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.PurchaseID = new SelectList(db.tbl_Purchase, "ID", "PartyInvoiceNo", tbl_Transaction.PurchaseID);
            ViewBag.SalesID = new SelectList(db.tbl_Sale, "SaleID", "InvoiceID", tbl_Transaction.SalesID);
            ViewBag.VendorID = new SelectList(db.tbl_vendor, "ID", "Name", tbl_Transaction.VendorID);
            ViewBag.PaymentID = new SelectList(db.tbl_Payment, "ID", "PaymentDate", tbl_Transaction.PaymentID);
            return View(tbl_Transaction);
        }

        // POST: Transaction/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AutoID,ID,VendorID,PurchaseID,SalesID,EntryID,EntryDate,EntryType,Amount,Status,CreatedDatetime,TransactionType,TransactionRef,PaymentID")] tbl_Transaction tbl_Transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Transaction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PurchaseID = new SelectList(db.tbl_Purchase, "ID", "PartyInvoiceNo", tbl_Transaction.PurchaseID);
            ViewBag.SalesID = new SelectList(db.tbl_Sale, "SaleID", "InvoiceID", tbl_Transaction.SalesID);
            ViewBag.VendorID = new SelectList(db.tbl_vendor, "ID", "Name", tbl_Transaction.VendorID);
            ViewBag.PaymentID = new SelectList(db.tbl_Payment, "ID", "PaymentDate", tbl_Transaction.PaymentID);
            return View(tbl_Transaction);
        }

        // GET: Transaction/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Transaction tbl_Transaction = await db.tbl_Transaction.FindAsync(id);
            if (tbl_Transaction == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Transaction);
        }

        // POST: Transaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            tbl_Transaction tbl_Transaction = await db.tbl_Transaction.FindAsync(id);
            db.tbl_Transaction.Remove(tbl_Transaction);
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
