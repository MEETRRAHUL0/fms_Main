using FMS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FMS.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()

        {

            return View();
        }

        public PartialViewResult LoadMenu()
        {
            var menus = MenuHelper.LoadMenu();
            return PartialView("LeftMenu", menus.menuItems);
        }
        
    }
}