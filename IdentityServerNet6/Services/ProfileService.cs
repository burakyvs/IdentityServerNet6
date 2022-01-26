using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServerApi.Services
{
    public class ProfileService : IProfileService
    {
        public ProfileService()
        {
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            string acr_values = context?.ValidatedRequest?.Raw.Get("acr_values");

            var claims = new List<Claim>
                {
                    new Claim("FullName", "sadf"),
                };

            context.IssuedClaims.AddRange(claims);

            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;

            return Task.FromResult(0);
        }
    }
}
