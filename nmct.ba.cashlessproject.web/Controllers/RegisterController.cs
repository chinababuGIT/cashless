using nmct.ba.cashlessproject.model.ITBedrijf;
using nmct.ba.cashlessproject.web.Helper;
using nmct.ba.cashlessproject.web.Models.OrganisationModel;
using nmct.ba.cashlessproject.web.PresentationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.ba.cashlessproject.web.Controllers
{
    [Authorize]
    public class RegisterController : Controller
    {
        // GET: Register
        [HttpGet]
        public ActionResult Index(string sort="",string show="not-empty",bool asc=true,bool desc=false)
        {
            if (ModelState.IsValid)
            {
                PMOrganisationRegister pm = new PMOrganisationRegister();
                //cookie ophalen
                if (HttpContext.Request.Cookies!=null)
                {
                    bool old = false;
                    HttpCookie cookie = HttpContext.Request.Cookies["Old"];
                    old = GetCookieValue(cookie);
                    show = old == true ? "empty" : "all";
                    ViewBag.old = old;
                }


                //controles querystrings
                pm.Registers = DAOrganisationRegister.LoadWithEmptyOrganisations().Where(m => m.RegisterExpiresDate > TimeConverter.ToUnixTimeStamp(DateTime.Now)).OrderBy(r=>r.OrganisationID).ToList<OrganisationRegister>();// alle wat onderaan niet geldt
                pm.EmptyRegisters = DAOrganisationRegister.LoadWithEmptyOrganisations().Where(m => m.OrganisationID == 0 && m.RegisterExpiresDate > TimeConverter.ToUnixTimeStamp(DateTime.Now)).OrderBy(r => r.OrganisationID).ToList<OrganisationRegister>();
                if (sort == "old" && show == "all")
                {
                    pm.Registers = DAOrganisationRegister.LoadWithEmptyOrganisations().OrderBy(r=>r.OrganisationID).ToList<OrganisationRegister>();
                    pm.EmptyRegisters = DAOrganisationRegister.LoadWithEmptyOrganisations().Where(m => m.OrganisationID == 0).OrderBy(r=>r.OrganisationID).ToList<OrganisationRegister>();
                }
                if (sort=="old" && show=="empty")
                {
                    pm.Registers = DAOrganisationRegister.LoadWithEmptyOrganisations().Where(m => m.RegisterExpiresDate < TimeConverter.ToUnixTimeStamp(DateTime.Now)).OrderBy(r=>r.OrganisationID).ToList<OrganisationRegister>();
                    pm.EmptyRegisters = DAOrganisationRegister.LoadWithEmptyOrganisations().Where(m => m.OrganisationID == 0 && m.RegisterExpiresDate < TimeConverter.ToUnixTimeStamp(DateTime.Now)).OrderBy(r=>r.OrganisationID).ToList<OrganisationRegister>();
               
                }
       
               
                pm.Organisations = DAOrganisation.Load();
                return View(pm);
            }
            return RedirectToRoute("Home");


           
        }

        [HttpGet]
        public ActionResult New()
        {
            if (ModelState.IsValid)
            {
                PMOrganisationRegister pm = new PMOrganisationRegister();
                pm.SelectOrganisation = new SelectList(DAOrganisation.Load(), "ID", "Name");
                return View(pm);
            }
            return RedirectToAction("Index");


           

        }
        [HttpPost]
        public ActionResult New(PMOrganisationRegister pm)
        {
            if (ModelState.IsValid)
            {
                DAOrganisationRegister.Insert(pm.OrganisationRegisterNew);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

           

        }


        [HttpGet]
        public ActionResult Edit(string type="none",int id=0)
        {
            if (id == 0) return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                PMOrganisationRegister pm = new PMOrganisationRegister();

                OrganisationRegister orgReg = DAOrganisationRegister.LoadByRegister(id);
                if (orgReg == null) RedirectToAction("Index");

                pm.SelectOrganisation = new SelectList(DAOrganisation.Load(), "ID", "Name");
                pm.OrganisationRegisterNew = orgReg;
                pm.OrganisationID =orgReg.OrganisationID;

                switch (type)
                {
                    case "full": return View("Edit_full", pm);
                    case "organisation": return View("Edit_organisation", pm);
                    case "none": return View("Edit", pm);
                    default: return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
            

        }
        [HttpPost]
        public ActionResult Edit(PMOrganisationRegister pm)
        {
            if (ModelState.IsValid)
            {
                if (pm.OrganisationRegisterNew.RegisterPurchaseDate == 0 && pm.OrganisationRegisterNew.RegisterExpiresDate == 0)
                {
                    pm.OrganisationRegisterNew.RegisterPurchaseDate = TimeConverter.ToUnixTimeStamp(pm.PurchaseDate);
                    pm.OrganisationRegisterNew.RegisterExpiresDate = TimeConverter.ToUnixTimeStamp(pm.ExpiresDate);
                }



                if (pm.OrganisationRegisterNew.FromDate ==0 && pm.OrganisationRegisterNew.UntilDate==0)
                {
                    pm.OrganisationRegisterNew.FromDate = TimeConverter.ToUnixTimeStamp(pm.FromDate);
                    pm.OrganisationRegisterNew.UntilDate = TimeConverter.ToUnixTimeStamp(pm.UntilDate);
                }

               

                if (pm.OrganisationID!=0)
                {
                    pm.OrganisationRegisterNew.OrganisationID = pm.OrganisationID;
                    DAOrganisationRegister.Update(pm.OrganisationRegisterNew);
                }
                else
                {
                    DAOrganisationRegister.Update(pm.OrganisationRegisterNew);
                    DAOrganisationRegister.Delete(pm.OrganisationRegisterNew);
                }
                
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

           
        }

        [HttpGet]
        public ActionResult Delete(string type="none",int id=0)
        {
            if(id==0) return RedirectToAction("Index");
            if (ModelState.IsValid)
            {
                PMOrganisationRegister pm = new PMOrganisationRegister();
                pm.OrganisationRegisterNew = DAOrganisationRegister.LoadByRegister(id);
                if (pm == null) RedirectToAction("Index");

                if(pm.OrganisationRegisterNew.RegisterExpiresDate<TimeConverter.ToUnixTimeStamp(DateTime.Now)){
                    return RedirectToAction("Index");
                }
                switch (type)
                {
                    case "full": return View("Delete", pm);
                    case "organisation": return View("Delete", pm);
                    case "none": return View("Index");
                    default: return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");

        }
        [HttpPost]
        public ActionResult Delete(PMOrganisationRegister pm)
        {
            if (ModelState.IsValid)
            {
                pm.OrganisationRegisterNew.RegisterPurchaseDate = TimeConverter.ToUnixTimeStamp(pm.PurchaseDate);
                pm.OrganisationRegisterNew.RegisterExpiresDate = TimeConverter.ToUnixTimeStamp(pm.ExpiresDate);

                string type= HttpContext.Request.QueryString["type"].ToString();


                switch (type)
                {
                    case "full": DARegister.Delete(pm.OrganisationRegisterNew.RegisterID);//register expiresdate wordt opvandaag gezet
                        break;
                    case "organisation": DAOrganisationRegister.Delete(pm.OrganisationRegisterNew);//untildate aan vndaag gelijk stellen, organisatie blijft opgeslagen voor later gebruik
                        break;
                    case "none": return RedirectToAction("Index");
                    default: return RedirectToAction("Index");
                }


            }
            return RedirectToAction("Index");        

        }

        [HttpGet]
        public ActionResult Old()
        {
            bool old = false;
            if (HttpContext.Request.Cookies["Old"]!=null)
            {
                HttpCookie cookie = Request.Cookies["Old"];
                old = SetCookieValue(cookie);
                HttpContext.Response.SetCookie(cookie);
            }
            else
            {
                HttpCookie cookie = new HttpCookie("Old");
                cookie.Expires = DateTime.Now.AddHours(2);
                cookie.Value = Convert.ToString(0);
                HttpContext.Response.SetCookie(cookie);
            }

            return RedirectToAction("Index", new { sort = "old", show = old==true ? "empty":"all" });
          

        }

        private static bool SetCookieValue(HttpCookie cookie)
        {
            bool old = false;
            if (Convert.ToInt32(cookie.Value) == 0)
            {
                cookie.Value = Convert.ToString(1);
                old = true;
            }
            else
            {
                cookie.Value = Convert.ToString(0);
                old = false;
            }
            return old;
        }
        private static bool GetCookieValue(HttpCookie cookie)
        {
            bool old = false;
            if (Convert.ToInt32(cookie.Value) == 0)
            {
               
                old = false;
            }
            else
            {
               
                old = true;
            }
            return old;
        }


    }
}