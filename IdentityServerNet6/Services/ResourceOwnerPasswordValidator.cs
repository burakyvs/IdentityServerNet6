using IdentityModel;
using IdentityServer4;
using IdentityServer4.Validation;
using IdentityServerNet6.Constants;
using IdentityServerNet6.Entities;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace IdentityServerNet6.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _accessor;
        
        public ResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager, IHttpContextAccessor accessor)
        {
            _userManager = userManager;
            _accessor = accessor;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var existUser = await _userManager.FindByEmailAsync(context.UserName);

            if(existUser == null)
            {
                existUser = await _userManager.FindByNameAsync(context.UserName);

                if (existUser == null)
                {
                    var errors = new Dictionary<string, object>();
                    errors.Add("errors", new List<string> { "Wrong email/username or password." });
                    context.Result.CustomResponse = errors;

                    return;
                }
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(existUser, context.Password);

            if(!passwordCheck)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string> { "Wrong email/username or password." });
                context.Result.CustomResponse = errors;

                return;
            }

            //var roles = await _userManager.GetRolesAsync(existUser);

            //var claims_list = new List<Claim>();

            //foreach (var role in roles)
            //{
            //    claims_list.Add(new Claim(JwtClaimTypes.Role, role));
            //}

            //claims_list.Add(new Claim(JwtClaimTypes.Subject, existUser.Id.ToString()));
            //claims_list.Add(new Claim(JwtClaimTypes.Name, existUser.FullName));
            //claims_list.Add(new Claim(JwtClaimTypes.Email, existUser.Email));

            //await _accessor.HttpContext.SignInAsync(new IdentityServerUser(existUser.Id.ToString())
            //{
            //    DisplayName = existUser.UserName,
            //    AdditionalClaims = claims_list,
            //    AuthenticationTime = DateTime.Now
            //});

            context.Result = new GrantValidationResult(existUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
        }
    }
}
