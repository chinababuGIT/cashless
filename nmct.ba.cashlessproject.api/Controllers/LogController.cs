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
    public class LogController : ApiController
    {
        public int Post(Log log) {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DALogs.Insert(log, principal.Claims);
        }
        public List<Log> Get()
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DALogs.Load(principal.Claims);
        }
    }
}
