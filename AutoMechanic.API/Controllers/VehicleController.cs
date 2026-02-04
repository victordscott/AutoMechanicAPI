using AutoMechanic.Auth.Helpers;
using AutoMechanic.DataAccess.DTO;
using AutoMechanic.DataAccess.Models.Proc;
using AutoMechanic.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoMechanic.API.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class VehicleController(IVehicleService vehicleService) : ControllerBase
    {
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<ActionResult<Guid>> AddVehicle([FromBody] VehicleDTO vehicleDto)
        {
            var userId = AuthHelper.GetUserIdFromPrincipal(User);
            vehicleDto.CustomerId = userId;
            var vehicleId = await vehicleService.AddVehicleAsync(vehicleDto);
            return StatusCode(StatusCodes.Status200OK, vehicleId);
        }
        
        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleDTO>>> GetVehiclesByCustomerId()
        {
            var customerId = AuthHelper.GetUserIdFromPrincipal(User);
            var vehicles = await vehicleService.GetVehiclesByCustomerIdAsync(customerId);
            return Ok(vehicles);
        }

        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<ActionResult<VehicleWithFiles>> GetVehicleWithFiles([FromQuery] Guid vehicleId)
        {
            var result = await vehicleService.GetVehicleWithFilesAsync(vehicleId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
}
