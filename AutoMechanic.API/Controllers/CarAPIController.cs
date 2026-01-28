using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AutoMechanic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarAPIController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CarAPIController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("years")]
        public async Task<ActionResult<List<int>>> GetYears()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync("https://carapi.app/api/years/v2");

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var years = JsonSerializer.Deserialize<List<int>>(jsonContent);
                    return Ok(years);
                }

                return StatusCode((int)response.StatusCode, "Failed to retrieve years from external API");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
