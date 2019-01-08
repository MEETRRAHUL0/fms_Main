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
using Newtonsoft.Json;

namespace FMS.Controllers
{
    public class SaleController : Controller
    {
        private FMSExpEntities db = new FMSExpEntities();
        private static List<tbl_ItemStock> SalesItemStock { get; set; }
        // GET: Sale
        public async Task<ActionResult> Index()
        {
            var tbl_Sale = db.tbl_Sale.Include(t => t.tbl_vendor);
            return View(await tbl_Sale.OrderByDescending(q => q.AutoID).ToListAsync());
        }

        public PartialViewResult LoadItems()
        {
            return PartialView(SalesItemStock);
        }

        public PartialViewResult Viewtems()
        {
            return PartialView(SalesItemStock);
        }

        public JsonResult AddItemToSession(string request)
        {
            tbl_ItemStock UIinput = JsonConvert.DeserializeObject<tbl_ItemStock>(request);
            db.Configuration.ProxyCreationEnabled = false;
            UIinput.tbl_Items = db.tbl_Items.Where(q => q.ID == UIinput.ItemID).FirstOrDefault();
            SalesItemStock.Add(UIinput);
            ViewBag.tbl_ItemStock = SalesItemStock;
            Session["SalesItemStock"] = SalesItemStock;
            var TotalAmountBeforeTax = SalesItemStock.Select(q => q.TotalPriceBeforeTax).Sum();
            var TaxAmount = SalesItemStock.Select(q => q.GST).Sum();
            var TotalAmountAfterTax = SalesItemStock.Select(q => q.TotalPriceAfterTax).Sum();

            var total = (from p in SalesItemStock
                         group p by 1 into g
                         select (
                       new
                       {
                           TotalAmountBeforeTax = g.Sum(q => q.TotalPriceBeforeTax),
                           TaxAmount = g.Sum(q => q.GST),
                           TotalAmountAfterTax = g.Sum(q => q.TotalPriceAfterTax),
                       }
                         )).FirstOrDefault();


            return Json(new { SalesItemStock, total }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult CreateItemList(string SalesID, string InvoiceNo)
        {
            ViewBag.ItemID = new SelectList(db.tbl_Items, "ID", "Name");
            ViewBag.SaleID = SalesID;
            ViewBag.ID = SalesID + "-" + Helper.GenericHelper.GetMaxValue("tbl_ItemStock") + "-" + (SalesItemStock.Count + 1);
            ViewBag.CreatedDatetime = DateTime.Now;
            ViewBag.InvoiceNo = InvoiceNo;
            ViewBag.StockType = "OUT";
            return PartialView();
        }




        // GET: Sale/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Sale tbl_Sale = await db.tbl_Sale.FindAsync(id);
            if (tbl_Sale == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Sale);
        }

        // GET: Sale/Create
        public ActionResult Create()
        {
            SalesItemStock = new List<tbl_ItemStock>();
            ViewBag.SaleID = Helper.GenericHelper.GetMaxValue("tbl_Sale");
            ViewBag.CreatedDatetime = DateTime.Now;
            ViewBag.InvoiceID = Helper.GenericHelper.GetMaxValue("tbl_SalesInvoice");
            ViewBag.CustomerID = new SelectList(db.tbl_vendor, "ID", "Name");
            return View();
        }

        // POST: Sale/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AutoID,SaleID,InvoiceID,InvoiceDate,CustomerID,DiscountAfterTax,OtherChargeAfterTax,SubTotal,Tax,TotalValueafterTax,GrandTotal,CreatedDatetime")] tbl_Sale tbl_Sale)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Sale.Add(tbl_Sale);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.tbl_vendor, "CustomerID", "Name", tbl_Sale.CustomerID);
            return View(tbl_Sale);
        }

        public ActionResult AddPurchase(string request)
        {
            tbl_Sale tbl_Sale = JsonConvert.DeserializeObject<tbl_Sale>(request);
            if (SalesItemStock.Count > 0)
            {
                var t = JsonConvert.SerializeObject(tbl_Sale);

                db.tbl_Sale.Add(tbl_Sale);
                db.SaveChanges();

                SalesItemStock.ForEach(q => q.tbl_Items = null);

                db.tbl_ItemStock.AddRange(SalesItemStock);
                db.SaveChanges();

                tbl_Sequence NewSequenceValue = Helper.GenericHelper.GetNextUpdatedData("tbl_Sale");
                db.Entry(NewSequenceValue).State = EntityState.Modified;
                //db.SaveChanges();

                tbl_Sequence NewSequenceValuetbl_SalesInvoice = Helper.GenericHelper.GetNextUpdatedData("tbl_SalesInvoice");
                db.Entry(NewSequenceValuetbl_SalesInvoice).State = EntityState.Modified;
                //db.SaveChanges();

                tbl_Sequence NewSequenceValueItemStock = Helper.GenericHelper.GetNextUpdatedData("tbl_ItemStock");
                db.Entry(NewSequenceValueItemStock).State = EntityState.Modified;
                db.SaveChanges();

                //return RedirectToAction("Index");
                return JavaScript("window.location = '/Sale/index'");
            }
            else
            {
                ViewBag.CustomerID = new SelectList(db.tbl_vendor, "CustomerID", "Name", tbl_Sale.CustomerID);
                throw new InvalidOperationException("Please add item to continue!!");

            }
            //return RedirectToAction("Index");
        }

        // GET: Sale/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Sale tbl_Sale = await db.tbl_Sale.FindAsync(id);
            if (tbl_Sale == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.tbl_vendor, "CustomerID", "Name", tbl_Sale.CustomerID);
            return View(tbl_Sale);
        }

        // POST: Sale/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AutoID,SaleID,InvoiceID,InvoiceDate,CustomerID,DiscountAfterTax,OtherChargeAfterTax,SubTotal,Tax,TotalValueafterTax,GrandTotal,CreatedDatetime")] tbl_Sale tbl_Sale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Sale).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.tbl_vendor, "CustomerID", "Name", tbl_Sale.CustomerID);
            return View(tbl_Sale);
        }

        // GET: Sale/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Sale tbl_Sale = await db.tbl_Sale.FindAsync(id);
            if (tbl_Sale == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Sale);
        }

        // POST: Sale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            tbl_Sale tbl_Sale = await db.tbl_Sale.FindAsync(id);
            db.tbl_Sale.Remove(tbl_Sale);
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
