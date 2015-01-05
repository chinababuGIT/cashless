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
    public class ProductController : ApiController
    {
        private const string ERROR = "Er was een probleem bij de request";
        private const string ERROR_PARAM = "Parameter is leeg";
        public List<Product> Get()
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DAProduct.Load(principal.Claims);
        }

        public Product Get(int? id)
        {
            if (!id.HasValue) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ERROR_PARAM));

            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            Product product = DAProduct.Load(id.Value, principal.Claims);
            if (product == null) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, ERROR));
            return product;
        }
        public HttpResponseMessage Put(Product product)
        {
            try
            {
                if (product == null) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ERROR_PARAM));
                ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
                int id = DAProduct.Update(product, principal.Claims);
                if (id == 0) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, ERROR));

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR));
            }
        }

        public HttpResponseMessage Post(Product product)
        {
            try
            {
                if (product == null) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ERROR_PARAM));
                ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
                int id = DAProduct.Insert(product, principal.Claims);
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
            try
            {
                if (!id.HasValue) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ERROR_PARAM));


                ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
                DAProduct.Remove(id.Value, principal.Claims);

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR));
            }
        }
    }
}
