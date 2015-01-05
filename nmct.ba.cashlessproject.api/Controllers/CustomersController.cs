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
    public class CustomersController : ApiController
    {
        public List<Customer> Get()
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DACustomers.Load(principal.Claims);
        }
        public Customer Get(int id)
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DACustomers.Load(id,principal.Claims);
        }
        public int Put(Customer customer)
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DACustomers.Update(customer, principal.Claims);
        }

        public int Post(Customer customer)
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DACustomers.Insert(customer, principal.Claims);
        }
    }
}
