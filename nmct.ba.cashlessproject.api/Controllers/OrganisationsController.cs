using nmct.ba.cashlessproject.api.Models;
using nmct.ba.cashlessproject.model.ITBedrijf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace nmct.ba.cashlessproject.api.Controllers
{
    [Authorize]
    public class OrganisationsController : ApiController
    {

        public List<Organisation> Get()
        {
            return null;
        }
     

    }
}
