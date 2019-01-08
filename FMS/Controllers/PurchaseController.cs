using FMS.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FMS.Controllers
{
    public class PurchaseController : Controller
    {
        private FMSExpEntities db = new FMSExpEntities();
        private static List<tbl_ItemStock> PurchaseItemStock { get; set; }
        //private static List<tbl_ItemStock> PurchaseItemStockforsaving { get; set; }

        // GET: Purchase
        public async Task<ActionResult> Index()
        {
            var tbl_Purchase = db.tbl_Purchase.Include(t => t.tbl_PaymentMode).Include(t => t.tbl_vendor);
            return View(await tbl_Purchase.OrderByDescending(q => q.AutoID).ToListAsync());
        }

        public PartialViewResult LoadItems()
        {
            return PartialView(PurchaseItemStock);
        }

        public PartialViewResult Viewtems()
        {
            return PartialView(PurchaseItemStock);
        }

        [HttpGet]
        public PartialViewResult CreateItemList(string PurchaseID, string PartyInvoiceNo)
        {
            ViewBag.ItemID = new SelectList(db.tbl_Items, "ID", "Name");
            ViewBag.PurchaseID = PurchaseID;
            ViewBag.ID = PurchaseID + "-" + Helper.GenericHelper.GetMaxValue("tbl_ItemStock") + "-" + (PurchaseItemStock.Count + 1);
            ViewBag.CreatedDatetime = DateTime.Now;
            ViewBag.InvoiceNo = PartyInvoiceNo;
            ViewBag.StockType = "IN";
            return PartialView();
        }



        [HttpPost]
        public JsonResult GetItemDetails(string id)
        {
            var StockDtl = db.vw_StockAvailable.Where(q => q.ItemsID == id).FirstOrDefault();
            var ItemGstDtl = db.tbl_Items.Where(q => q.ID == id).FirstOrDefault();

            var StockIn = 0;
            var StockOut = 0;
            var StockAvailable = 0;
            var HSNSACNum = "NA";
            if (StockDtl != null)
            {
                StockIn = StockDtl.StockIn;
                StockOut = StockDtl.StockOut;
                StockAvailable = StockDtl.StockAvailable ?? 0;
                HSNSACNum = StockDtl.HSN_SAC_NO ?? "NA";
            }

            decimal SGST = 0.0m;
            decimal CGST = 0.0m;
            decimal GST = 0.0m;
            string Type = "NA";
            string MeasuringUnit = "NA";
            if (ItemGstDtl != null)
            {
                SGST = ItemGstDtl.tbl_GST.SGST == null ? SGST : ItemGstDtl.tbl_GST.SGST.Value;
                CGST = ItemGstDtl.tbl_GST.CGST == null ? SGST : ItemGstDtl.tbl_GST.CGST.Value;
                GST = ItemGstDtl.tbl_GST.GST == null ? SGST : ItemGstDtl.tbl_GST.GST.Value;
                MeasuringUnit = ItemGstDtl.tbl_ItemUnits.MeasuringUnits ?? "NA";
                Type = ItemGstDtl.tbl_ItemType.ItemType ?? "NA";
            }
            return Json(new { StockIn, StockOut, StockAvailable, HSNSACNum, SGST, CGST, GST, MeasuringUnit, Type }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddItemToSession(string request)
        {
            tbl_ItemStock UIinput = JsonConvert.DeserializeObject<tbl_ItemStock>(request);

            db.Configuration.ProxyCreationEnabled = false;
            UIinput.tbl_Items = db.tbl_Items.Where(q => q.ID == UIinput.ItemID).FirstOrDefault();
            PurchaseItemStock.Add(UIinput);
            ViewBag.tbl_ItemStock = PurchaseItemStock;
            Session["PurchaseItemStock"] = PurchaseItemStock;
            var TotalAmountBeforeTax = PurchaseItemStock.Select(q => q.TotalPriceBeforeTax).Sum();
            var TaxAmount = PurchaseItemStock.Select(q => q.GST).Sum();
            var TotalAmountAfterTax = PurchaseItemStock.Select(q => q.TotalPriceAfterTax).Sum();

            var total = (from p in PurchaseItemStock
                         group p by 1 into g
                         select (
                       new
                       {
                           TotalAmountBeforeTax = g.Sum(q => q.TotalPriceBeforeTax),
                           TaxAmount = g.Sum(q => q.GST),
                           TotalAmountAfterTax = g.Sum(q => q.TotalPriceAfterTax),
                       }
                         )).FirstOrDefault();


            return Json(new { PurchaseItemStock, total }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteItemFromSession(string ID)
        {

            PurchaseItemStock.RemoveAll(q => q.ID == ID);

            ViewBag.tbl_ItemStock = PurchaseItemStock;
            Session["PurchaseItemStock"] = PurchaseItemStock;
            var TotalAmountBeforeTax = PurchaseItemStock.Select(q => q.TotalPriceBeforeTax).Sum();
            var TaxAmount = PurchaseItemStock.Select(q => q.GST).Sum();
            var TotalAmountAfterTax = PurchaseItemStock.Select(q => q.TotalPriceAfterTax).Sum();

            var total = (from p in PurchaseItemStock
                         group p by 1 into g
                         select (
                       new
                       {
                           TotalAmountBeforeTax = g.Sum(q => q.TotalPriceBeforeTax),
                           TaxAmount = g.Sum(q => q.GST),
                           TotalAmountAfterTax = g.Sum(q => q.TotalPriceAfterTax),
                       }
                         )).FirstOrDefault();


            return Json(new { PurchaseItemStock, total }, JsonRequestBehavior.AllowGet);
        }

        // GET: Purchase/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Purchase tbl_Purchase = await db.tbl_Purchase.FindAsync(id);
            if (tbl_Purchase == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Purchase);
        }

        // GET: Purchase/Create
        public ActionResult Create()
        {
            PurchaseItemStock = new List<tbl_ItemStock>();

            Session["PurchaseItemStock"] = new List<tbl_ItemStock>();
            ViewBag.PaymentMode = new SelectList(db.tbl_PaymentMode, "ID", "Mode");
            ViewBag.PartyName = new SelectList(db.tbl_vendor, "ID", "Name");
            ViewBag.ID = Helper.GenericHelper.GetMaxValue("tbl_Purchase");
            ViewBag.CreatedDatetime = DateTime.Now;
            ViewBag.tbl_ItemStock = PurchaseItemStock;
            return View();
        }

        // POST: Purchase/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AutoID,ID,PartyInvoiceDate,PurchaseDate,PartyInvoiceNo,PartyName,PaymentDueAfter,ReverseCharge,tbl_ItemStock,Remark,DiscountAmount,OtherCharge,TotalAmountBeforeTax,TaxAmount,TotalAmountAfterTax,RoundOff,GrandTotal,PurchaseBook,PaymentMode,ScanCopy,CreatedDatetime")] tbl_Purchase tbl_Purchase, tbl_ItemStock tbl_ItemStock)
        {
            if (ModelState.IsValid)
            {
                var t = JsonConvert.SerializeObject(tbl_Purchase);
                db.tbl_Purchase.Add(tbl_Purchase);
                await db.SaveChangesAsync();

                tbl_Sequence NewSequenceValue = Helper.GenericHelper.GetNextUpdatedData("tbl_Purchase");
                db.Entry(NewSequenceValue).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            ViewBag.PaymentMode = new SelectList(db.tbl_PaymentMode, "ID", "Mode", tbl_Purchase.PaymentMode);
            ViewBag.PartyName = new SelectList(db.tbl_vendor, "ID", "Name", tbl_Purchase.PartyName);
            return View(tbl_Purchase);
        }

        public ActionResult AddPurchase(string request)
        {
            tbl_Purchase tbl_Purchase = JsonConvert.DeserializeObject<tbl_Purchase>(request);
            if (PurchaseItemStock.Count > 0)
            {
                var t = JsonConvert.SerializeObject(tbl_Purchase);

                db.tbl_Purchase.Add(tbl_Purchase);

                PurchaseItemStock.ForEach(q => q.tbl_Items = null);
                db.tbl_ItemStock.AddRange(PurchaseItemStock);

                tbl_Sequence NewSequenceValue = Helper.GenericHelper.GetNextUpdatedData("tbl_Purchase");
                db.Entry(NewSequenceValue).State = EntityState.Modified;

                tbl_Sequence NewSequenceValueItemStock = Helper.GenericHelper.GetNextUpdatedData("tbl_ItemStock");
                db.Entry(NewSequenceValueItemStock).State = EntityState.Modified;

                db.SaveChanges();

                // 1 hold --  only save purchase & Item with status hold

                // 2  CREDIT PURCHASE --  one transaction entry with credit (out) with unpaid status

                // 3  CASH PAY  -- one transaction entry with credit (out)/ Paid
                //             one payment Entry with full amount 
                //             one transaction entry with Debit (in) with payment ID

                // 4 MULTI MODE PAY -- one transaction entry with credit (out)
                //             one payment Entry
                //             one transaction entry with Debit (in)

                if (tbl_Purchase.PaymentMode != 1)
                {
                    int amount = tbl_Purchase.Amount;
                    tbl_Transaction tbl_Transaction = new tbl_Transaction()
                    {
                        ID = Helper.GenericHelper.GetMaxValue("tbl_Transaction"),

                        CreatedDatetime = tbl_Purchase.CreatedDatetime,
                        EntryDate = tbl_Purchase.PurchaseDate.ToString(),

                        EntryType = EnumHelper.TransactionEntryType.Purchase.ToString(),  // "Purchase", // Purchase/Sales/Recipt/Payment
                        Status = null,  ///Paid/Unpaid/Pending/Paid Against entries : // Recive  Against entries 
                        TransactionType = null,  ///Debit/ credit --  For Real: Debit what comes in, credit what goes out.
                        TransactionRef = null,

                        EntryID = tbl_Purchase.ID,
                        PurchaseID = tbl_Purchase.ID,
                        SalesID = null,

                        Amount = amount,

                        VendorID = tbl_Purchase.tbl_vendor.ID

                    };
                    db.Entry(tbl_Transaction).State = EntityState.Modified;

                }

                return JavaScript("window.location = '/purchase/index'");
            }
            else
            {
                ViewBag.PaymentMode = new SelectList(db.tbl_PaymentMode, "ID", "Mode", tbl_Purchase.PaymentMode);
                ViewBag.PartyName = new SelectList(db.tbl_vendor, "ID", "Name", tbl_Purchase.PartyName);
                throw new InvalidOperationException("Please add item to continue!!");

            }
            //return RedirectToAction("Index");
        }

        // GET: Purchase/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Purchase tbl_Purchase = await db.tbl_Purchase.FindAsync(id);
            if (tbl_Purchase == null)
            {
                return HttpNotFound();
            }
            ViewBag.PaymentMode = new SelectList(db.tbl_PaymentMode, "ID", "Mode", tbl_Purchase.PaymentMode);
            ViewBag.PartyName = new SelectList(db.tbl_vendor, "ID", "Name", tbl_Purchase.PartyName);
            return View(tbl_Purchase);
        }

        // POST: Purchase/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AutoID,ID,PartyInvoiceDate,PurchaseDate,PartyInvoiceNo,PartyName,PaymentDueAfter,ReverseCharge,Remark,DiscountAmount,OtherCharge,TotalAmountBeforeTax,TaxAmount,TotalAmountAfterTax,RoundOff,GrandTotal,PurchaseBook,PaymentMode,ScanCopy,CreatedDatetime")] tbl_Purchase tbl_Purchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Purchase).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.PaymentMode = new SelectList(db.tbl_PaymentMode, "ID", "Mode", tbl_Purchase.PaymentMode);
            ViewBag.PartyName = new SelectList(db.tbl_vendor, "ID", "Name", tbl_Purchase.PartyName);
            return View(tbl_Purchase);
        }

        // GET: Purchase/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Purchase tbl_Purchase = await db.tbl_Purchase.FindAsync(id);
            if (tbl_Purchase == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Purchase);
        }

        // POST: Purchase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            tbl_Purchase tbl_Purchase = await db.tbl_Purchase.FindAsync(id);
            db.tbl_Purchase.Remove(tbl_Purchase);
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