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
    public class StockExpirydetailsController : Controller
    {
        private FMSExpEntities db = new FMSExpEntities();

        // GET: StockExpirydetails
        public async Task<ActionResult> Index()
        {
            return View(await db.vw_StockExpirydetails.ToListAsync());
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
