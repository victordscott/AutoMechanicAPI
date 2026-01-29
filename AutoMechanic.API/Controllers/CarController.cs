using AutoMechanic.CarAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
            return await carAPIService.GetYears();
        }
    }
}
