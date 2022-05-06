using BugTracksV3.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace BugTracker.Services.Factories;

public class BTUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
{
    public BTUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
                                        RoleManager<IdentityRole> roleManager,
                                        IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
    {

    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        ClaimsIdentity identity = await base.GenerateClaimsAsync(user);
        identity.AddClaim(new Claim("CompanyId", user.CompanyId.ToString()));
        return identity;
    }
}