using AutoMapper;
using AutoMechanic.Auth.Models;
using AutoMechanic.Auth.Services;
using AutoMechanic.Auth.Services.Interfaces;
using AutoMechanic.Common.Enums;
using AutoMechanic.Common.Model;
using AutoMechanic.DataAccess.DirectAccess;
using AutoMechanic.DataAccess.EF.Models;
using AutoMechanic.DataAccess.Models;
using AutoMechanic.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AutoMechanic.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController(
        IAuthService authService,
        IUserService userService,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IPasswordHasher<ApplicationUser> passwordHasher,
        IMapper mapper
    ) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<AuthResponse> Login(LoginModel loginModel)
        {
            var authResponse = await authService.LoginAsync(loginModel);
            return authResponse;
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpPost]
        public async Task<AuthResponse> LoginWithOTPCode(LoginModel loginModel)
        {
            var authResponse = await authService.LoginByOTPCode(loginModel);
            return authResponse;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<BoolResponse> EmailAddressInUse([FromQuery] string emailAddress)
        {
            var user = await userManager.FindByEmailAsync(emailAddress);
            return new BoolResponse
            {
                BoolValue = user is not null
            };
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ApiResponse> RegisterCustomer(RegistrationModel regModel)
        {
            var password = "Vst!1971@"; // generate a random password

            var existingUser = await userManager.FindByEmailAsync(regModel.EmailAddress);

            if (existingUser is not null)
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorMessage = "User with that email address already exists. Please use a different email address",
                    ErrorCode = (int)ApiErrorCode.RegisterEmailAddressExists
                };
            }

            var createResult = await userManager.CreateAsync(new ApplicationUser
            {
                UserName = regModel.EmailAddress,
                //NormalizedUserName = regModel.EmailAddress.ToUpper(),
                Email = regModel.EmailAddress,
                //NormalizedEmail = regModel.EmailAddress.ToUpper(),
                PhoneNumber = regModel.PhoneNumber,
                FirstName = regModel.FirstName,
                LastName = regModel.LastName,
                State = regModel.State,
                Country = regModel.Country,
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
                TimeZoneAbbrev = regModel.TimeZoneAbbrev,
                DateCreated = DateTime.UtcNow
            }, password);
            if (createResult.Succeeded)
            {
                var user = await userManager.FindByNameAsync(regModel.EmailAddress);
                var addToRoleResult = await userManager.AddToRoleAsync(user, "Customer");
                if (addToRoleResult.Succeeded)
                {
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
                        ErrorMessage = $"Error: {addToRoleResult.Errors.First().Code} - {addToRoleResult.Errors.First().Description}",
                        ErrorCode = (int)ApiErrorCode.SetUserRoleError
                    };
                }
            }
            else
            {
                return new ApiResponse
                {
                    Success = false,
                    ErrorMessage = $"Error: {createResult.Errors.First().Code} - {createResult.Errors.First().Description}",
                    ErrorCode = (int)ApiErrorCode.CreateCustomerError
                };
            }
        }
    }
}
