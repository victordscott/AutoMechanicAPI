using AutoMechanic.Auth.Helpers;
using AutoMechanic.Auth.Models;
using AutoMechanic.Auth.Services;
using AutoMechanic.Common.Enums;
using AutoMechanic.Common.Model;
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
        IUserService userService
    ) : ControllerBase
    {
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
