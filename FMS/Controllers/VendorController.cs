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
    public class VendorController : Controller
    {
        private FMSExpEntities db = new FMSExpEntities();

        // GET: Vendor
        public async Task<ActionResult> Index()
        {
            return View(await db.tbl_vendor.OrderByDescending(q=>q.AutoID).ToListAsync());
        }

        // GET: Vendor/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_vendor tbl_vendor = await db.tbl_vendor.FindAsync(id);
            if (tbl_vendor == null)
            {
                return HttpNotFound();
            }
            return View(tbl_vendor);
        }

        // GET: Vendor/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateSupplier()
        {
            ViewBag.VendorType = "Supplier";
            ViewBag.ID = Helper.GenericHelper.GetMaxValue("tbl_Supplier");
            ViewBag.CreatedDatetime = DateTime.Now;
            return View("Create");
        }

        [HttpGet]
        public ActionResult CreateCustomer()
        {
            ViewBag.VendorType = "Customer";
            ViewBag.ID = Helper.GenericHelper.GetMaxValue("tbl_Customer");
            ViewBag.CreatedDatetime = DateTime.Now;

            return View("Create");
        }

        // POST: Vendor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AutoID,ID,Name,ContactPerson,ContactNo,PhoneNo,VendorType,Type,Place,DOB,GSTNo,TINNo,PANNo,CINNo,AdhaarNo,OpeningBalance,OpeningBalanceType,OpeningBalanceDate,CreditLimit,CreditPeriod,CreditInterestRate,DebitInterestRate,CreatedDatetime,Photo,Remark,SuretyPerson,SuretyPersonContactNo,SuretyPersonAddress")] tbl_vendor tbl_vendor)
        {
            if (ModelState.IsValid)
            {
                db.tbl_vendor.Add(tbl_vendor);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tbl_vendor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateSupplier([Bind(Include = "AutoID,ID,Name,ContactPerson,ContactNo,PhoneNo,VendorType,Type,Place,DOB,GSTNo,TINNo,PANNo,CINNo,AdhaarNo,OpeningBalance,OpeningBalanceType,OpeningBalanceDate,CreditLimit,CreditPeriod,CreditInterestRate,DebitInterestRate,CreatedDatetime,Photo,Remark,SuretyPerson,SuretyPersonContactNo,SuretyPersonAddress")] tbl_vendor tbl_vendor)
        {
            if (ModelState.IsValid)
            {
                db.tbl_vendor.Add(tbl_vendor);
                await db.SaveChangesAsync();

                tbl_Sequence NewSequenceValue = Helper.GenericHelper.GetNextUpdatedData("tbl_Supplier");
                db.Entry(NewSequenceValue).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(tbl_vendor);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateCustomer([Bind(Include = "AutoID,ID,Name,ContactPerson,ContactNo,PhoneNo,VendorType,Type,Place,DOB,GSTNo,TINNo,PANNo,CINNo,AdhaarNo,OpeningBalance,OpeningBalanceType,OpeningBalanceDate,CreditLimit,CreditPeriod,CreditInterestRate,DebitInterestRate,CreatedDatetime,Photo,Remark,SuretyPerson,SuretyPersonContactNo,SuretyPersonAddress")] tbl_vendor tbl_vendor)
        {
            if (ModelState.IsValid)
            {
                db.tbl_vendor.Add(tbl_vendor);
                await db.SaveChangesAsync();

                tbl_Sequence NewSequenceValue = Helper.GenericHelper.GetNextUpdatedData("tbl_Customer");
                db.Entry(NewSequenceValue).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(tbl_vendor);
        }

        // GET: Vendor/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_vendor tbl_vendor = await db.tbl_vendor.FindAsync(id);
            if (tbl_vendor == null)
            {
                return HttpNotFound();
            }
            return View(tbl_vendor);
        }

        // POST: Vendor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AutoID,ID,Name,ContactPerson,ContactNo,PhoneNo,VendorType,Type,Place,DOB,GSTNo,TINNo,PANNo,CINNo,AdhaarNo,OpeningBalance,OpeningBalanceType,OpeningBalanceDate,CreditLimit,CreditPeriod,CreditInterestRate,DebitInterestRate,CreatedDatetime,Photo,Remark,SuretyPerson,SuretyPersonContactNo,SuretyPersonAddress")] tbl_vendor tbl_vendor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_vendor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tbl_vendor);
        }

        // GET: Vendor/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_vendor tbl_vendor = await db.tbl_vendor.FindAsync(id);
            if (tbl_vendor == null)
            {
                return HttpNotFound();
            }
            return View(tbl_vendor);
        }

        // POST: Vendor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            tbl_vendor tbl_vendor = await db.tbl_vendor.FindAsync(id);
            db.tbl_vendor.Remove(tbl_vendor);
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
