using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exercise1.Controllers
{
    public class TryitController : Controller
    {
        // GET: Tryit
        public ActionResult Index()
        {
            ViewBag.KellyName = "黃凱筠";
            ViewData["Vivian"] = "張文薰";
            return View();
        }

        public ActionResult PassOne()
        {
            ViewBag.Emma = "張岑瑜";
            TempData["Fourth"] = "張展御-老四";
            TempData["Fifth"] = "真是夠了喔";
            return RedirectToAction("Index");
            // RedirectToAction("Index");
            //return View("Index");
        }


        //[HttpPost]
        //public ActionResult PassOne(string name)
        //{
        //    ViewBag.Emma = "張岑瑜";
        //    TempData["Fourth"] = "張展御";
        //    return RedirectToAction("Index");
        //    // RedirectToAction("Index");
        //    //return View("Index");
        //}
        public ActionResult CheckInput(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                TempData["Error"] = "不得空白！ ";
                return RedirectToAction("DemoInput");
            }

            ViewBag.Name = "張小三";
            return View();
        }

        public ActionResult DemoInput()
        {
            return View();
        }


    }
}