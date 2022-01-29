using Diplom2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Diplom2
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        public ApplicationUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            IOptions<IdentityOptions> options,
            RoleManager<IdentityRole> roleManager
            ) : base(userManager, roleManager, options)
        {
        }
        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            if(principal.FindFirst("ThemeSite") != null)
                ((ClaimsIdentity)principal.Identity).RemoveClaim(principal.FindFirst("ThemeSite"));
            else
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[]
                {
                    
                    new Claim("ThemeSite", user.ThemeSite.ToString()),
                });
            }
            return principal;
        }
    }
}
