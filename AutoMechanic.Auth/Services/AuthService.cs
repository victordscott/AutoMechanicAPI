using AutoMechanic.Auth.Models;
using AutoMechanic.Auth.Services.Interfaces;
using AutoMechanic.Common.Enums;
using AutoMechanic.DataAccess.EF.Models;
using AutoMechanic.DataAccess.Models;
using AutoMechanic.Services.Services;
using AutoMechanic.Services.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.Auth.Services
{
    public class AuthService(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        IUserService userService
    ) : IAuthService
    {
        public async Task<AuthResponse> LoginAsync(LoginModel loginModel)
        {
            var user = await userManager.FindByNameAsync(loginModel.UserName);
            if (user != null && await userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                return await GetToken(user);

                //var userRoles = await userManager.GetRolesAsync(user);
                //var userDetail = await userService.GetUserByIdAsync(user.Id);

                //var authClaims = new List<Claim>
                //{
                //    new Claim(ClaimTypes.Name, user.UserName),
                //    new Claim("username", user.UserName),
                //    new Claim("name", $"{user.FirstName} {user.LastName}"),
                //    new Claim("role", userRoles[0]),    // users will only have one role
                //    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //    new Claim(JwtRegisteredClaimNames.ZoneInfo, userDetail.TimeZoneName)
                //};

                //foreach (var userRole in userRoles)
                //{
                //    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                //}

                //var jwtToken = tokenService.GenerateAccessToken(authClaims);
                //var refreshToken = await tokenService.GenerateRefreshToken(user.Id);
                //var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

                //return new AuthResponse
                //{
                //    AccessToken = token,
                //    RefreshToken = refreshToken
                //};
            }
            else
            {
                return new AuthResponse
                {
                    Succeeded = false,
                    Message = "Invalid credentials"
                };
            }
        }

        public async Task<AuthResponse> LoginByOTPCode(LoginModel loginModel)
        {
            var user = await userManager.FindByEmailAsync(loginModel.EmailAddress);
            if (user != null && await userService.VerifyUserLoginOTPCodeAsync(user.Id, loginModel.OTPCode))
            {
                return await GetToken(user);
            }
            else
            {
                return new AuthResponse
                {
                    Succeeded = false,
                    Message = "Invalid credentials"
                };
            }
        }

        public async Task<AuthResponse> RefreshTokenAsync(TokenModel tokenModel)
        {
            var principal = tokenService.GetPrincipalFromToken(tokenModel.AccessToken, validateLifetime: false);
            var userName = principal.Identity.Name;
            var userId = Guid.Parse(principal.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value);

            var refreshTokenExists = await userService.UserRefreshTokenExists(userId, tokenModel.RefreshToken);
            if (!refreshTokenExists)
            {
                return new AuthResponse
                {
                    Succeeded = false,
                    Message = "Invalid refresh token"
                };
            }

            var jwtToken = tokenService.GenerateAccessToken(principal.Claims.ToList());
            var refreshToken = await tokenService.GenerateRefreshToken(userId);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            // delete the login model for the used refresh token
            await userService.DeleteUserRefreshToken(userId, tokenModel.RefreshToken);

            return new AuthResponse()
            {
                AccessToken = token,
                RefreshToken = refreshToken
            };
        }

        public async Task<ApiResponse> GetOTPCodeForLogin(string emailAddress)
        {
            var user = await userService.GetUserByUserEmailAsync(emailAddress);
            if (user is not null)
            {
                var otpCode = GenerateOtpCode();
                await userService.InsertUserLoginOTPCodeAsync(user.UserId.Value, otpCode);
                return new ApiResponse
                {
                    Success = true
                };
            }
            else
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorCode = (int)ApiErrorCode.InvalidEmailAddress,
                    ErrorMessage = "Invalid email address."
                };
            }
        }

        private async Task<AuthResponse> GetToken(ApplicationUser user)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            var userDetail = await userService.GetUserByIdAsync(user.Id);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("username", user.UserName),
                    new Claim("name", $"{user.FirstName} {user.LastName}"),
                    new Claim("role", userRoles[0]),    // users will only have one role
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.ZoneInfo, userDetail.TimeZoneName)
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var jwtToken = tokenService.GenerateAccessToken(authClaims);
            var refreshToken = await tokenService.GenerateRefreshToken(user.Id);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return new AuthResponse
            {
                AccessToken = token,
                RefreshToken = refreshToken
            };
        }

        private string GenerateOtpCode(int length = 6)
        {
            Random random = new Random();
            return random.Next(0, (int)Math.Pow(10, length)).ToString("D" + length);
        }
    }
}
