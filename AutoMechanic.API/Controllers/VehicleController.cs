using AutoMechanic.Auth.Helpers;
using AutoMechanic.DataAccess.DTO;
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
            return StatusCode(StatusCodes.Status201Created, vehicleId);
        }
        
        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleDTO>>> GetVehicleByCustomerId()
        {
            var customerId = AuthHelper.GetUserIdFromPrincipal(User);
            var vehicles = await vehicleService.GetVehiclesByCustomerIdAsync(customerId);
            if (vehicles == null || !vehicles.Any())
            {
                return NotFound();
            }
            return Ok(vehicles);
        }
    }
}
