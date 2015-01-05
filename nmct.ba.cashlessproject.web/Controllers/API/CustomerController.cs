using nmct.ba.cashlessproject.web.Models;
using nmct.ba.cashlessproject.model.Kassa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web;

namespace nmct.ba.cashlessproject.web.API.Controllers
{
    [Authorize]
    public class CustomerController : ApiController
    {
        private const string ERROR="Er was een probleem bij de request";
        private const string ERROR_PARAM = "Parameter is leeg";
        public List<Customer> Get()
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DACustomer.Load(principal.Claims);
        }
        public Customer Get(int? id)
        {
            if (!id.HasValue) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ERROR_PARAM));

            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            Customer customer= DACustomer.Load(id.Value,principal.Claims);
            if (customer == null) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, ERROR));
            return customer;
        }
       
        public Customer Get(string registerNumber)
        {
            if (registerNumber=="") throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ERROR_PARAM));

            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            Customer customer = DACustomer.Load(registerNumber, principal.Claims);
            if (customer == null) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, ERROR));
            return customer;
        }
        public HttpResponseMessage Put(Customer customer)
        {
            try
            {
                if (customer == null) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ERROR_PARAM));
                ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
                int id = DACustomer.Update(customer, principal.Claims);
                if (id == 0) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, ERROR));

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR));
            }
        }
        public HttpResponseMessage Post(Customer customer)
        {
            try
            {
                if (customer == null) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest,ERROR_PARAM )); 
                ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
                int id=DACustomer.Insert(customer, principal.Claims);
                if (id == 0) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, ERROR));

                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError,ERROR));
            }
            
        }  
        public HttpResponseMessage Delete(int? id)
        {
            try
            {
                if (!id.HasValue) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ERROR_PARAM));

                ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
                DACustomer.Remove(id.Value, principal.Claims);

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR));
            }
        }


    }
}
