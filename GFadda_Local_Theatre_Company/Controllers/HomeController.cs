using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GFadda_Local_Theatre_Company.Controllers
{
    public class HomeController : Controller
    {
        //create the splash screen
        public ActionResult SplashScreen()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        //create contact page 
        public ActionResult Contact()
        {
            ViewBag.Message = "Local Theatre Comtact Page";

            return View();
        }
    }
}