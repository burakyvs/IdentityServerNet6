using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServerApi.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IHttpContextAccessor _accessor;
        public ProfileService(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var isAuthenticated = _accessor.HttpContext?.User.Identity?.IsAuthenticated;
            if (isAuthenticated.HasValue && isAuthenticated.Value)
            {
                // @TODO
                //var claims = new List<Claim>
                //{
                //    _accessor.HttpContext.User.Claims.First(i => i.Type == JwtClaimTypes.Name),
                //    _accessor.HttpContext.User.Claims.First(i => i.Type == JwtClaimTypes.Role)
                //};

                //context.IssuedClaims.AddRange(claims);

                return Task.FromResult(0);
            }

            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            var a = context.Subject;
            return Task.FromResult(0);
        }
    }
}
