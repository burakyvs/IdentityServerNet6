using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServerNet6.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityServerApi.Services
{
    public sealed class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly UserManager<ApplicationUser> _userMgr;
        private readonly RoleManager<ApplicationRole> _roleMgr;

        public ProfileService(
            UserManager<ApplicationUser> userMgr,
            RoleManager<ApplicationRole> roleMgr,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory)
        {
            _userMgr = userMgr;
            _roleMgr = roleMgr;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            string sub = context.Subject.GetSubjectId();
            ApplicationUser user = await _userMgr.FindByIdAsync(sub);
            ClaimsPrincipal userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);

            // gets claims from ResourceOwnerPasswordValidator, GrantValidationResult claim list.
            //var resourceOwnerClaims = context.Subject.Claims.ToList();

            // gets claims from SignUp method in IdentityController.
            List<Claim> claims = userClaims.Claims.ToList();

            // RequestedClaimTypes comes from ApiResources.
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            // if roles has special claims. (example: admin has connect_admin_panel claim but moderator doesn't have it.)
            if (_userMgr.SupportsUserRole)
            {
                IList<string> roles = await _userMgr.GetRolesAsync(user);
                foreach (var roleName in roles)
                {
                    if (_roleMgr.SupportsRoleClaims)
                    {
                        ApplicationRole role = await _roleMgr.FindByNameAsync(roleName);
                        if (role != null)
                        {
                            claims.AddRange(await _roleMgr.GetClaimsAsync(role));
                        }
                    }
                }
            }

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            string sub = context.Subject.GetSubjectId();
            ApplicationUser user = await _userMgr.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
