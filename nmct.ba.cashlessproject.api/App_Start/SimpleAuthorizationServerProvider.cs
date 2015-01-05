using Microsoft.Owin.Security.OAuth;
using nmct.ba.cashlessproject.api.Models;
using nmct.ba.cashlessproject.model.ITBedrijf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace nmct.ba.cashlessproject.api.App_Start
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
            context.Validated(id);
            return Task.FromResult(0);

        }
    }
}