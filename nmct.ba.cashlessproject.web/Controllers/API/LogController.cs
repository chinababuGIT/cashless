using nmct.ba.cashlessproject.web.Models;
using nmct.ba.cashlessproject.model.Kassa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace nmct.ba.cashlessproject.web.API.Controllers
{
    [Authorize]
    public class LogController : ApiController
    {
        private const string ERROR = "Er was een probleem bij de request";
        private const string ERROR_PARAM = "Parameter is leeg";
        public List<Log> Get()
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DALog.Load(principal.Claims);
        }
        public HttpResponseMessage Put(Log log)
        {
            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR));
        }
        public HttpResponseMessage Post(Log log)
        {
            try
            {
                if (log == null) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ERROR_PARAM));
                ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
                int id = DALog.Insert(log, principal.Claims);
                //if (id == 0) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, ERROR));

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR));
            }
        }
        public HttpResponseMessage Delete(int? id)
        {
            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR));
        }
    }
}
