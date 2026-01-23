using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.Auth.Helpers
{
    public static class AuthHelper
    {
        public static Guid GetUserIdFromPrincipal(ClaimsPrincipal principal)
        {
            return Guid.Parse(principal.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value);
        }
    }
}
