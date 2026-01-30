using AutoMechanic.CarAPI.Models;
using AutoMechanic.CarAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoMechanic.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class CarController(
        ICarAPIService carAPIService
    ) : ControllerBase
    {
        [HttpGet]
        public async Task<List<int>> GetYears()
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(10));
            return await carAPIService.GetYears();
        }

        [HttpGet]
        public async Task<List<CarAPIMake>> GetMakes([FromQuery] string year)
        {
            return await carAPIService.GetMakes(year);
        }

        [HttpGet]
        public async Task<List<CarAPIModel>> GetModels([FromQuery] string year, [FromQuery] string make)
        {
            return await carAPIService.GetModels(year, make);
        }

        [HttpGet]
        public async Task<List<CarAPISubModel>> GetSubModels([FromQuery] string year, [FromQuery] string make, [FromQuery] string model)
        {
            return await carAPIService.GetSubModels(year, make, model);
        }

        [HttpGet]
        public async Task<ActionResult<CarAPIVINResponse>> GetVIN([FromQuery] string vin)
        {
            var result = await carAPIService.GetVIN(vin);
            return result is null ? NotFound() : result;
        }
    }
}
