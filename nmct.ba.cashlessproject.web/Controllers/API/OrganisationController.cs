using nmct.ba.cashlessproject.model.ITBedrijf;
using nmct.ba.cashlessproject.web.Models.OrganisationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace nmct.ba.cashlessproject.web.Controllers.API
{
    [Authorize]
    public class OrganisationController : ApiController
    {
        private const string ERROR = "Er was een probleem bij de request";
        private const string ERROR_PARAM = "Parameter is leeg";
       
        public Organisation Get()
        {
            ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
            return DAOrganisation.GetUser(principal.Claims);
        }

        public HttpResponseMessage Put(Organisation organisation)
        {
            try
            {
                if (organisation == null) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, ERROR_PARAM));
                ClaimsPrincipal principal = RequestContext.Principal as ClaimsPrincipal;
                int id = DAOrganisation.ChangePassword(organisation,principal.Claims);
                if (id == 0) throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, ERROR));

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR));
            }
        }
        public HttpResponseMessage Post(Organisation organisation)
        {
            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR));
        }
        public HttpResponseMessage Delete(int? id)
        {
            throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ERROR));
        }
        
    }
}
