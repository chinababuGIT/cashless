using nmct.ba.cashlessproject.model.ITBedrijf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.ba.cashlessproject.web.PresentationModels
{
    public class PMRegister
    {
        public PMRegister()
        {
            this.RegisterNew = new Register();
        }
        public List<OrganisationRegister> Registers { get; set; }
        public List<OrganisationRegister> EmptyRegisters { get; set; }
        public List<Organisation> Organisations{ get; set;}
        
        public Register RegisterNew { get; set; }
    }
}