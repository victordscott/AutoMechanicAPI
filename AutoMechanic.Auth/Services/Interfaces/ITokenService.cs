using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.Auth.Services.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateAccessToken(List<Claim> claims);
        Task<string> GenerateRefreshToken(Guid userId);
        ClaimsPrincipal GetPrincipalFromToken(string token, bool validateLifetime);
    }
}
