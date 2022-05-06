using System.Security.Claims;
using System.Security.Principal;

namespace BugTracksV3.Extensions
{
    public static class IdentityExtensions
    {
        //Only once instance is needed (static)
        public static int? GetCompanyId(this IIdentity identity)
        {
            //ClaimsIdentity implements the IIdentity
            Claim claim = ((ClaimsIdentity)identity).FindFirst("CompanyId");

            return (claim != null) ? int.Parse(claim.Value) : null;
        }
    }
}