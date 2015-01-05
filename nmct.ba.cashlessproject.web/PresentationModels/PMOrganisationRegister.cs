using nmct.ba.cashlessproject.model.ITBedrijf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.ba.cashlessproject.web.PresentationModels
{
    public class PMOrganisationRegister
    {
        public PMOrganisationRegister()
        {
            this.OrganisationRegisterNew = new OrganisationRegister();

        }
        public OrganisationRegister OrganisationRegisterNew { get; set; }
        public int OrganisationID { get; set; }
        public List<OrganisationRegister> Registers { get; set; }
        public List<OrganisationRegister> EmptyRegisters { get; set; }
        public List<Organisation> Organisations { get; set; }
        public SelectList SelectOrganisation { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpiresDate { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime UntilDate { get; set; }

    }
}