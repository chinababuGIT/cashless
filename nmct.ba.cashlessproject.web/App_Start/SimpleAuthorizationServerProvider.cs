using Microsoft.Owin.Security.OAuth;
using nmct.ba.cashlessproject.web.Models;
using nmct.ba.cashlessproject.model.ITBedrijf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using nmct.ba.cashlessproject.web.Models.OrganisationModel;

namespace nmct.ba.cashlessproject.web.App_Start
{
    public class SimpleAuthorizationServerProvider:OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            Organisation organisation = DAOrganisation.Login(context.UserName, context.Password);
            if (organisation == null)
            {
                context.Rejected();
                return Task.FromResult(0);
            }


            ClaimsIdentity id = new ClaimsIdentity(context.Options.AuthenticationType);
            id.AddClaim(new Claim("DbUsername", organisation.DbUsername));
            id.AddClaim(new Claim("DbPassword", organisation.DbPassword));
            id.AddClaim(new Claim("DbName", organisation.DbName));
            id.AddClaim(new Claim("ID", organisation.ID.ToString()));
         
            context.Validated(id);
            return Task.FromResult(0);

        }
    }
}