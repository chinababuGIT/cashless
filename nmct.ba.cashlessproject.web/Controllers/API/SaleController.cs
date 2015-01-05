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
    public class SaleController : ApiController
    {
        private const string ERROR = "Er was een probleem bij de request";
        private const string ERROR_PARAM = "Parameter is leeg";
        public List<Sale> Get()
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DASale.Load(principal.Claims);
        }

        public Sale Get(int? id)
        {
            if (!id.HasValue) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Parameter id is niet meegegeven"));

            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            Sale sale = DASale.Load(id.Value, principal.Claims);
            if (sale == null) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, ERROR));
            return sale;
        }

        public HttpResponseMessage Put(Sale sale)
        {
            try
            {
                if (sale == null) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ERROR_PARAM));
                ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
                int id = DASale.Update(sale, principal.Claims);
                if (id == 0) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, ERROR));

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR));
            }
        }

        public HttpResponseMessage Post(Sale sale)
        {
            try
            {
                if (sale == null) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ERROR_PARAM));
                ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
                int id = DASale.Insert(sale, principal.Claims);
                if (id == 0) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, ERROR));

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
