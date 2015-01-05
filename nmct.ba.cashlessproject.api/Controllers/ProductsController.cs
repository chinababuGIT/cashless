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
    public class ProductsController : ApiController
    {
        public List<Product> Get()
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DAProducts.Load(principal.Claims);
        }

        public Product Get(int id)
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DAProducts.Load(id,principal.Claims);
        }
        public int Put(Product product)
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DAProducts.Update(product,principal.Claims);
        }

        public int Post(Product product)
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DAProducts.Insert(product,principal.Claims);
        }
        public void Delete(int id)
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            DAProducts.Remove(id,principal.Claims);
        }
    }
}
