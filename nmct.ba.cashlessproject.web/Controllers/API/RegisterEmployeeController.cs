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
    public class RegisterEmployeeController : ApiController
    {
        private const string ERROR = "Er was een probleem bij de request";
        private const string ERROR_PARAM = "Parameter is leeg";

        public List<RegisterEmployee> Get()
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DARegisterEmployee.Load(principal.Claims);
        }
        public RegisterEmployee Get(int? id)
        {
            if (!id.HasValue) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ERROR_PARAM));

            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            RegisterEmployee registerEmployee = DARegisterEmployee.Load(id.Value, principal.Claims);
            if (registerEmployee == null) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, ERROR));
            return registerEmployee;
        }
        public HttpResponseMessage Put(RegisterEmployee registerEmployee)
        {
            try
            {
                if (registerEmployee == null) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ERROR_PARAM));
                ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
                int id = DARegisterEmployee.Update(registerEmployee, principal.Claims);
                //if (id == 0) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, ERROR));

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR));
            }
        }
        public HttpResponseMessage Post(RegisterEmployee registerEmployee)
        {
            try
            {
                if (registerEmployee == null) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ERROR_PARAM));
                ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
                int id = DARegisterEmployee.Insert(registerEmployee, principal.Claims);
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
