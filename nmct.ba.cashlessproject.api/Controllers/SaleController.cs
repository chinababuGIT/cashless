using nmct.ba.cashlessproject.api.Models;
using nmct.ba.cashlessproject.model.Kassa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace nmct.ba.cashlessproject.api.Controllers
{
    [Authorize]
    public class SaleController : ApiController
    {
        public List<_Sale> Get()
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DASales.Load(principal.Claims);
        }

        public _Sale Get(int id)
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DASales.Load(id, principal.Claims);
        }

        public int Put(_Sale sale)
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DASales.Update(sale, principal.Claims);
        }

        public int Post(_Sale sale)
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DASales.Insert(sale, principal.Claims);
        }

    }
}
