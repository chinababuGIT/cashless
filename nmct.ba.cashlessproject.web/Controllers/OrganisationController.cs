using nmct.ba.cashlessproject.model.ITBedrijf;
using nmct.ba.cashlessproject.web.Helper;
using nmct.ba.cashlessproject.web.Models;
using nmct.ba.cashlessproject.web.Models.OrganisationModel;
using nmct.ba.cashlessproject.web.PresentationModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace nmct.ba.cashlessproject.web.Controllers
{
    [Authorize]
    public class OrganisationController : Controller
    {
        // GET: Organisation
        [HttpGet]
        public ActionResult Index()
        {
            if (ModelState.IsValid)
            {
                PMOrganisation pm = new PMOrganisation();
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
                PMOrganisation pm = new PMOrganisation();
                return View(pm);
            }
            return RedirectToRoute("Home");

        }
        [HttpPost]
        public ActionResult New(PMOrganisation pm)
        {
                HttpPostedFileBase file = pm.File;
                if (file != null && file.ContentLength > 0)
                {
                    pm.OrganisationNew.Logo = GetPhoto(file);
                }
                string dbName = "Customer_" + pm.OrganisationNew.Name.Replace(' ', '_').ToLower();
                string dbUsername = "Customer_" + pm.OrganisationNew.Name.Replace(' ', '_').ToLower();
                string dbPassword = Cryptography.Encrypt(Membership.GeneratePassword(5, 0));

                pm.OrganisationNew.Username.Trim().ToLower().Replace(' ','_');
                pm.OrganisationNew.DbName = dbName;
                pm.OrganisationNew.DbUsername = dbUsername;
                pm.OrganisationNew.DbPassword = dbPassword;
                pm.OrganisationNew.Password = Cryptography.Encrypt(pm.OrganisationNew.Password);

                int i = DAOrganisation.Insert(pm.OrganisationNew);
                if (i == 0)
                {
                    return RedirectToAction("Error", new { error = "De gegevens konden niet worden geupload worden" });
                }
                else
                {
                    return RedirectToAction("Index");
                }

        }


        [HttpGet]
        public ActionResult Edit(int id=0)
        {
            if (id == 0) return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                Organisation organisation = DAOrganisation.Load(id);
                if (organisation == null) return RedirectToAction("Index");

                PMOrganisation pm = new PMOrganisation();
                pm.OrganisationNew = organisation;
                pm.OrganisationNew.Password = "";
                return View(pm);
            }
           

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(PMOrganisation pm)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = pm.File;
                if (file != null && file.ContentLength > 0)
                {
                    pm.OrganisationNew.Logo = GetPhoto(file);
                }

                //string dbName = "Customer_" + pm.OrganisationNew.Name.Replace(' ', '_').ToLower();
                //string dbUsername = "Customer_" + pm.OrganisationNew.Name.Replace(' ', '_').ToLower();
                //string dbPassword = Cryptography.Encrypt(pm.OrganisationNew.Name.Replace(' ', '_').ToLower() + Membership.GeneratePassword(12, 1));

                //pm.OrganisationNew.DbName = dbName;
                //pm.OrganisationNew.DbUsername = dbUsername;
                //pm.OrganisationNew.DbPassword = dbPassword;
                pm.OrganisationNew.Password = Cryptography.Encrypt(pm.OrganisationNew.Password);



                int i = DAOrganisation.Update(pm.OrganisationNew);
                if (i == 0)
                {
                    return RedirectToAction("Error", new { error = "De gegevens konden niet worden geupload worden" });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            if (ModelState.IsValid)
            {
                Organisation organisation = DAOrganisation.Load(id);
                if (organisation == null) return RedirectToAction("Index");

                PMOrganisation pm = new PMOrganisation();
                pm.OrganisationNew = organisation;
                return View(pm);
            }
           

            return RedirectToAction("Index");
        }
       

        [HttpGet]
        public ActionResult Remove(int id)
        {
            if (ModelState.IsValid)
            {
                Organisation organisation = DAOrganisation.Load(id);
                if (organisation == null) return RedirectToAction("Index");
                return ViewBag(organisation);
            }
           return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult Error(string error)
        {
            ViewBag.Error = error;
            return View("Error");

        }



        private byte[] GetPhoto(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, (int)fs.Length);
            fs.Close();

            return data;
        }
        private byte[] GetPhoto(HttpPostedFileBase file)
        {
            
            byte[] data = new byte[file.ContentLength];
            using (var fs= file.InputStream)
            {
                var offset = 0;
                do
                {
                    offset += fs.Read(data, offset, data.Length - offset);
                    
                } while (offset<data.Length);
            }
           

            return data;
        }

      

    }
}