using nmct.ba.cashlessproject.model.ITBedrijf;
using nmct.ba.cashlessproject.web.Models.OrganisationModel;
using nmct.ba.cashlessproject.web.PresentationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.ba.cashlessproject.web.Controllers
{
    public class LogController : Controller
    {
        // GET: Log
        [HttpGet]
        public ActionResult Index(int kassa=0)
        {
            if (ModelState.IsValid)
            {
                PMRegisterLog pm = new PMRegisterLog();
                pm.RegisterLogs = DARegisterLog.Load();
                pm.SelectRegister = new SelectList(DARegister.Load(), "ID", "Name");
                pm.RegisterLogNew.RegisterID = kassa;

                if (kassa != 0)
                {
                    List<OrganisationRegister> lijst = DAOrganisationRegister.Load().Where(r => r.RegisterID == kassa).ToList<OrganisationRegister>();
                    
                    pm.RegisterLogs = pm.RegisterLogs.Where(m => m.RegisterID == kassa).ToList<RegisterLog>();
                }
                return View(pm);
            }
            return RedirectToRoute("Home");
        }
        [HttpPost]
        public ActionResult Index(PMRegisterLog pm)
        {
            return RedirectToAction("Index", new { kassa = pm.RegisterLogNew.RegisterID });
        }
    }
}