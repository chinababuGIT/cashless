using nmct.ba.cashlessproject.web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.ba.cashlessproject.web.Controllers
{
    [Authorize]
    public class UpdateController : Controller
    {
        
        // GET: Update
        [HttpGet]
        public ActionResult Index()
        {
            return View("Ok");
        }
        [HttpPost]
        public ActionResult Index(string test="")
        {
            Sync.Start();
            return RedirectToAction("Index", "Home");
            //return RedirectToRoute("Default");
        }
    }
}