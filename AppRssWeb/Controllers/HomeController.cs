using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FreeAppRss.AppRssDatabase;

namespace FreeAppRss.AppRssWeb.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewBag.Title = "FreeAppRss.com";
            List<App> apps = DatabaseAccessor.GetAllApps();
            return View(apps);
        }
    }
}
