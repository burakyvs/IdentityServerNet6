using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerNet6.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
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
    }
}
