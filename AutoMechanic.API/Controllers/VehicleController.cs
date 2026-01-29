using AutoMechanic.Auth.Helpers;
using AutoMechanic.DataAccess.DTO;
using AutoMechanic.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoMechanic.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class VehicleController(IVehicleService vehicleService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> AddVehicle([FromBody] VehicleDTO vehicleDto)
        {
            var userId = AuthHelper.GetUserIdFromPrincipal(User);
            vehicleDto.CustomerId = userId;
            var vehicleId = await vehicleService.AddVehicleAsync(vehicleDto);
            return StatusCode(StatusCodes.Status201Created, vehicleId);
        }
    }
}
