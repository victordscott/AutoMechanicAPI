using AutoMapper;
using AutoMechanic.Auth.Models;
using AutoMechanic.Auth.Services;
using AutoMechanic.Auth.Services.Interfaces;
using AutoMechanic.Common.Enums;
using AutoMechanic.DataAccess.DirectAccess;
using AutoMechanic.DataAccess.EF.Models;
using AutoMechanic.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoMechanic.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController(IAuthService authService, IMapper mapper) : ControllerBase
    {
        [HttpPost]
        public async Task<AuthResponse> Login(LoginModel loginModel)
        {
            var authResponse = await authService.LoginAsync(loginModel);
            return authResponse;
        }

        [HttpPost]
        public async Task<AuthResponse> RefreshToken(TokenModel tokenModel)
        {
            var authResponse = await authService.RefreshTokenAsync(tokenModel);
            return authResponse;
        }

        [HttpPost]
        public ApplicationUser CreateConsultant(CreateConsultantRequest createConsultantRequest)
        {
            var user = mapper.Map<ApplicationUser>(createConsultantRequest);
            return user;
        }

        [HttpPost]
        public async Task<ApiResponse> GetOTPCodeForLogin(LoginModel loginModel)
        {
            var otpCode = await authService.GetOTPCodeForLogin(loginModel.EmailAddress);
            if (!string.IsNullOrEmpty(otpCode)) {
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

        [HttpPost]
        public async Task<AuthResponse> LoginWithOTPCode(LoginModel loginModel)
        {
            var authResponse = await authService.LoginByOTPCode(loginModel);
            return authResponse;
        }
    }
}
