using AutoMechanic.Auth.Helpers;
using AutoMechanic.Auth.Models;
using AutoMechanic.Auth.Services;
using AutoMechanic.DataAccess.DTO;
using AutoMechanic.DataAccess.EF.Models;
using AutoMechanic.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
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
