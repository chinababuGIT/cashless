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
    public class EmployeesController : ApiController
    {
        // api/Employees?id=1
        // api/Employees/1
        public List<Employee> Get()
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DAEmployees.Load(principal.Claims);
        }

        public Employee Get(int id)
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DAEmployees.Load(id,principal.Claims);
        }
        public int Put(Employee product)
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DAEmployees.Update(product,principal.Claims);
        }

        public int Post(Employee product)
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DAEmployees.Insert(product,principal.Claims);
        }
        public void Delete(int id)
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            DAEmployees.Remove(id,principal.Claims);
        }
    }
}
