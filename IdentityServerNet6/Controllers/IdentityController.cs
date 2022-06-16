using IdentityModel;
using IdentityServerNet6.Constants;
using IdentityServerNet6.Entities;
using IdentityServerNet6.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace IdentityServerNet6.Controllers
{
    [ApiController]
    [Route("auth")]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public IdentityController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet("[action]")]
        public IActionResult CheckTokenExpiration(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken;
            try
            {
                jwtSecurityToken = tokenHandler.ReadJwtToken(accessToken);
            }
            catch
            {
                return NotFound("Token not found.");
            }

            if (jwtSecurityToken.ValidTo < DateTime.UtcNow.AddSeconds(10))
                return Unauthorized("Token is expired.");
            else
            {
                TimeSpan ts = jwtSecurityToken.ValidTo - DateTime.UtcNow.AddSeconds(10);
                return Ok($"Remaining token access time: {ts}.");
            }

        }

        [HttpPost("[action]")]
        [Route("kayitol")]
        public async Task<IActionResult> SignUp([FromBody]SignUpModel signUpModel)
        {

            var user = new ApplicationUser
            {
                UserName = signUpModel.UserName,
                Email = signUpModel.Email,
                FullName = signUpModel.FullName
            };

            var userCreation = await _userManager.CreateAsync(user, signUpModel.Password);

            if (userCreation.Succeeded)
            {
                await _userManager.AddClaimsAsync(user, new List<Claim>
                {
                    new Claim(JwtClaimTypes.Role, ApplicationRoles.UserRole),
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(JwtClaimTypes.Name, user.UserName)
                }); 

                var response = new
                {
                    Success = true,
                    Message = "User successfully registered.",
                    Info = new { 
                        UserName = signUpModel.UserName,
                        Email = signUpModel.Email,
                    },
                };
                return Ok(JsonSerializer.Serialize(response, options: new JsonSerializerOptions 
                { 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                }));
            }
            else
            {
                var response = new
                {
                    Success = false,
                    Message = "User registration failed.",
                    Errors = userCreation.Errors
                };
                return BadRequest(JsonSerializer.Serialize(response, options: new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                }));
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetUser()
        {

            var userIdClaim = User.Claims.FirstOrDefault(i => i.Type == JwtRegisteredClaimNames.Sub);

            if (userIdClaim == null)
            {
                return BadRequest();
            }

            var user = _userManager.FindByIdAsync(userIdClaim.Value);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(JsonSerializer.Serialize(user, options: new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            }));
        }
    }
}
