using AutoMechanic.Auth.Services.Interfaces;
using AutoMechanic.Configuration.Options;
using AutoMechanic.Services.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.Auth.Services
{
    public class TokenService(IOptions<JWTOptions> jwtOptions, IUserService userService) : ITokenService
    {
        public JwtSecurityToken GenerateAccessToken(List<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Secret));

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Value.ValidIssuer,
                audience: jwtOptions.Value.ValidAudience,
                expires: DateTime.Now.AddMinutes(jwtOptions.Value.TokenValidMinutes),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

        public async Task<string> GenerateRefreshToken(Guid userId)
        {
            var randomNumber = new byte[32];
            string refreshToken = null;
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken = Convert.ToBase64String(randomNumber);
            }

            await userService.SaveUserRefreshTokenAsync(userId, refreshToken, DateTime.UtcNow.AddMinutes(jwtOptions.Value.RefreshTokenValidMinutes));

            return refreshToken;
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token, bool validateLifetime)
        {
            //DefaultInboundClaimTypeMap
            //MapInboundClaims
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Secret)),
                ValidateLifetime = validateLifetime // if false, we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.MapInboundClaims = false;  // fixes sub claim issue
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
    }
}
