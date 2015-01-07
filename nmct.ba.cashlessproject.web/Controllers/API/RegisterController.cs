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
    public class RegisterController : ApiController
    {
        private const string ERROR = "Er was een probleem bij de request";
        private const string ERROR_PARAM = "Parameter is leeg";
        public List<Register> Get()
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DARegister.Load(principal.Claims);
        }
        public Register Get(int? id)
        {
            if (!id.HasValue) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ERROR_PARAM));

            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            Register register = DARegister.Load(id.Value, principal.Claims);
            if (register == null) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, ERROR));
            return register;
        }
        public HttpResponseMessage Put(Register register)
        {
            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR));
        }
        public HttpResponseMessage Post(Register register)
        {
            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR));
        }
        public HttpResponseMessage Delete(int? id)
        {
            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR));
        }
    }
}
