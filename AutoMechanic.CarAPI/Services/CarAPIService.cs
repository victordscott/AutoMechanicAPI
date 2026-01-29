using AutoMechanic.CarAPI.Services.Interfaces;
using AutoMechanic.Configuration.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoMechanic.CarAPI.Services
{
    public class CarAPIService(
        IHttpClientFactory httpClientFactory,
        IOptions<CarAPIOptions> carAPIOptions
    ) : ICarAPIService
    {
        public async Task<List<int>> GetYears()
        {
            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{carAPIOptions.Value.BaseUrl}api/years/v2");

            // TODO: could do this
            //response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var years = JsonSerializer.Deserialize<List<int>>(jsonContent);
                return years;
            }
            else
            {
                // TODO: logging
                throw new Exception("CarAPI call failed");
            }
        }
    }
}
