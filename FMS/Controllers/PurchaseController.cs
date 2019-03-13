using FMS.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using static FMS.Helper.EnumHelper;

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
            using (var transaction = db.Database.BeginTransaction()) // new System.Transactions.TransactionScope())
            {
                tbl_Purchase tbl_Purchase = JsonConvert.DeserializeObject<tbl_Purchase>(request);
                if (PurchaseItemStock.Count > 0)
                {
                    var t = JsonConvert.SerializeObject(tbl_Purchase);

                    db.tbl_Purchase.Add(tbl_Purchase);

                    PurchaseItemStock.ForEach(q => q.tbl_Items = null);
                    db.tbl_ItemStock.AddRange(PurchaseItemStock);

                    tbl_Sequence NewSequenceValue = Helper.GenericHelper.GetNextUpdatedData(SequenceTable.tbl_Purchase.ToString());
                    db.Entry(NewSequenceValue).State = EntityState.Modified;

                    tbl_Sequence NewSequenceValueItemStock = Helper.GenericHelper.GetNextUpdatedData(SequenceTable.tbl_ItemStock.ToString());
                    db.Entry(NewSequenceValueItemStock).State = EntityState.Modified;

                    //db.SaveChanges();

                    // 1 hold --  only save purchase & Item with status hold

                    // 2  CREDIT PURCHASE --  one transaction entry with credit (out) with unpaid status

                    // 3  CASH PAY  -- one transaction entry with credit (out)/ Paid
                    //             one payment Entry with full amount
                    //             one transaction entry with Debit (in) with payment ID

                    // 4 MULTI MODE PAY -- one transaction entry with credit (out)
                    //             one payment Entry
                    //             one transaction entry with Debit (in)

                    if (tbl_Purchase.PaymentMode.Value != PaymentMode.HOLD)  // not hold
                    {
                        decimal amount = tbl_Purchase.Amount;
                        string creditTID = Helper.GenericHelper.GetMaxValue(SequenceTable.tbl_TransactionCredit.ToString());
                        tbl_Transaction tbl_Transactioncredit = new tbl_Transaction()
                        {
                            ID = creditTID,

                            CreatedDatetime = tbl_Purchase.CreatedDatetime,
                            EntryDate = tbl_Purchase.PurchaseDate.ToString(),

                            EntryType = TransactionEntryType.Purchase.ToString(),  //"Purchase", // Purchase/Sales/Recipt/Payment
                            Status = TransactionStatus.Unpaid,  ///Paid/Unpaid/Pending/Paid Against entries : // Recive  Against entries
                            TransactionType = TransactionType.Credit.ToString(),  ///Debit/ credit --  For Real: Debit what comes in, credit what goes out.
                            //TransactionRef = null,

                            EntryID = tbl_Purchase.ID,
                            PurchaseID = tbl_Purchase.ID,
                            //SalesID = null,

                            Amount = tbl_Purchase.GrandTotal,

                            VendorID = tbl_Purchase.PartyName
                        };
                        db.tbl_Transaction.Add(tbl_Transactioncredit);
                        //db.SaveChanges();

                        tbl_Sequence NewSequenceValuetbl_Transaction = Helper.GenericHelper.GetNextUpdatedData("tbl_TransactionCredit");
                        db.Entry(NewSequenceValuetbl_Transaction).State = EntityState.Modified;

                        //db.SaveChanges();
                        if (tbl_Purchase.PaymentMode == 2)  //CREDIT PURCHASE
                        {
                        }
                        else if (tbl_Purchase.PaymentMode == 3)  //CASH PAY
                        {
                            string tbl_PaymentID = Helper.GenericHelper.GetMaxValue("tbl_Payment");
                            tbl_Payment tbl_Payment = new tbl_Payment()
                            {
                                ID = tbl_PaymentID,
                                Amount = tbl_Purchase.GrandTotal,
                                CreatedDatetime = tbl_Purchase.CreatedDatetime,

                                PartyName = tbl_Purchase.PartyName,
                                PaymentDate = tbl_Purchase.PurchaseDate.ToString(),
                                PaymentMode = "CASH",
                                //Remark = "",
                                //TransactionID = creditTID,
                            };
                            db.tbl_Payment.Add(tbl_Payment);

                            tbl_Sequence NewSequenceValuetbl_Payment = Helper.GenericHelper.GetNextUpdatedData("tbl_Payment");
                            db.Entry(NewSequenceValuetbl_Payment).State = EntityState.Modified;
                            //db.SaveChanges();

                            tbl_Transaction tbl_TransactionDebit = new tbl_Transaction()
                            {
                                ID = Helper.GenericHelper.GetMaxValue("tbl_TransactionDebit"),

                                CreatedDatetime = tbl_Purchase.CreatedDatetime,
                                EntryDate = tbl_Purchase.PurchaseDate.ToString(),

                                EntryType = TransactionEntryType.Purchase.ToString(),  // "Purchase", // Purchase/Sales/Recipt/Payment
                                Status = TransactionStatus.PaidAgainstEntries + tbl_Purchase.ID,  ///Paid/Unpaid/Pending/Paid Against entries : // Recive  Against entries
                                TransactionType = TransactionType.Debit.ToString(),  ///Debit/ credit --  For Real: Debit what comes in, credit what goes out.
                                TransactionRef = tbl_Purchase.ID,

                                EntryID = tbl_PaymentID,
                                //PurchaseID = null,
                                //SalesID = null,
                                PaymentID = tbl_PaymentID,

                                Amount = tbl_Purchase.GrandTotal,

                                VendorID = tbl_Purchase.PartyName
                            };
                            db.tbl_Transaction.Add(tbl_TransactionDebit);

                            tbl_Sequence NewSequenceValuetbl_Transaction2 = Helper.GenericHelper.GetNextUpdatedData("tbl_TransactionDebit");
                            db.Entry(NewSequenceValuetbl_Transaction2).State = EntityState.Modified;

                            //db.SaveChanges();

                            //var tbl_Transactionresult = db.tbl_Transaction.Where(q => q.ID == creditTID).FirstOrDefault();
                            tbl_Transactioncredit.Status = TransactionStatus.Paid;
                            //db.Entry(tbl_Transactionresult).State = EntityState.Modified;
                            //db.SaveChanges();
                        }
                        else if (tbl_Purchase.PaymentMode == 4)  //MULTI MODE PAY
                        {
                            string tbl_PaymentID = Helper.GenericHelper.GetMaxValue("tbl_Payment");
                            tbl_Payment tbl_Payment = new tbl_Payment()
                            {
                                ID = tbl_PaymentID,
                                Amount = amount,
                                CreatedDatetime = tbl_Purchase.CreatedDatetime,

                                PartyName = tbl_Purchase.PartyName,
                                PaymentDate = tbl_Purchase.PurchaseDate.ToString(),
                                PaymentMode = "CASH",
                                //Remark = "",
                                //TransactionID = creditTID,
                            };
                            db.tbl_Payment.Add(tbl_Payment);

                            tbl_Sequence NewSequenceValuetbl_Payment = Helper.GenericHelper.GetNextUpdatedData("tbl_Payment");
                            db.Entry(NewSequenceValuetbl_Payment).State = EntityState.Modified;
                            //db.SaveChanges();

                            tbl_Transaction tbl_TransactionDebit = new tbl_Transaction()
                            {
                                ID = Helper.GenericHelper.GetMaxValue("tbl_TransactionDebit"),

                                CreatedDatetime = tbl_Purchase.CreatedDatetime,
                                EntryDate = tbl_Purchase.PurchaseDate.ToString(),

                                EntryType = TransactionEntryType.Purchase.ToString(),  // "Purchase", // Purchase/Sales/Recipt/Payment
                                Status = TransactionStatus.PaidAgainstEntries + tbl_Purchase.ID,  ///Paid/Unpaid/Pending/Paid Against entries : // Recive  Against entries
                                TransactionType = TransactionType.Debit.ToString(),  ///Debit/ credit --  For Real: Debit what comes in, credit what goes out.
                                TransactionRef = tbl_Purchase.ID,

                                EntryID = tbl_PaymentID,
                                //PurchaseID = null,
                                //SalesID = null,
                                PaymentID = tbl_PaymentID,

                                Amount = amount,

                                VendorID = tbl_Purchase.PartyName
                            };
                            db.tbl_Transaction.Add(tbl_TransactionDebit);

                            tbl_Sequence NewSequenceValuetbl_Transaction2 = Helper.GenericHelper.GetNextUpdatedData("tbl_TransactionDebit");
                            db.Entry(NewSequenceValuetbl_Transaction2).State = EntityState.Modified;
                        }
                    }

                    bool saved = false;

                    try
                    {
                        db.SaveChanges();
                        saved = true;
                    }
                    catch (Exception e)
                    {
                        throw new InvalidOperationException(e.Message);
                    }
                    finally
                    {
                        if (saved)
                        {
                            transaction.Commit();
                        }
                    }

                    //if (tbl_Purchase.PaymentMode == 4)  //MULTI MODE PAY
                    //    return JavaScript("window.location = '/purchase/index'");
                    //else
                    return JavaScript("window.location = '/purchase/index'");
                }
                else
                {
                    ViewBag.PaymentMode = new SelectList(db.tbl_PaymentMode, "ID", "Mode", tbl_Purchase.PaymentMode);
                    ViewBag.PartyName = new SelectList(db.tbl_vendor, "ID", "Name", tbl_Purchase.PartyName);
                    throw new InvalidOperationException("Please add item to continue!!");
                }
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