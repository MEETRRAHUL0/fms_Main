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
    public class StockAvailableController : Controller
    {
        private FMSExpEntities db = new FMSExpEntities();

        // GET: StockAvailable
        public async Task<ActionResult> Index()
        {
            return View(await db.vw_StockAvailable.ToListAsync());
        }

        // GET: StockAvailable/Details/5
        

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
