using AutoMechanic.Auth.Helpers;
using AutoMechanic.Auth.Models;
using AutoMechanic.Auth.Services;
using AutoMechanic.Common.Enums;
using AutoMechanic.DataAccess.DTO;
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
    public class UserController(
        IUserService userService,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IPasswordHasher<ApplicationUser> passwordHasher
    ) : ControllerBase
    {
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
                    ErrorCode = (int) ApiErrorCode.RegisterEmailAddressExists
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

        [HttpGet]
        public async Task<UserDetail> GetProfile()
        {
            var userId = AuthHelper.GetUserIdFromPrincipal(User);
            var user = await userService.GetUserByIdAsync(userId);

            return user;
        }

        [HttpGet]
        public async Task<List<TimeZoneDTO>> GetTimeZones()
        {
            return await userService.GetTimeZonesAsync();
        }
    }
}
