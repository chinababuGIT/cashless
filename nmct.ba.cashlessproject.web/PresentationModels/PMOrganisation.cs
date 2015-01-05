using nmct.ba.cashlessproject.model.ITBedrijf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nmct.ba.cashlessproject.web.PresentationModels
{
    public class PMOrganisation
    {
        public PMOrganisation()
        {
            this.OrganisationNew = new Organisation();
        }

        public Organisation OrganisationNew { get; set; }
        public List<Organisation> Organisations { get; set; }
        public HttpPostedFileBase File { get; set; }
    }
}