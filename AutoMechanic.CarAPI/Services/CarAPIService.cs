using AutoMechanic.CarAPI.Models;
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

        public async Task<List<CarAPIMake>> GetMakes(string year)
        {
            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{carAPIOptions.Value.BaseUrl}api/makes/v2?year={year}");

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var makes = JsonSerializer.Deserialize<CarAPIMakesResponse>(jsonContent);
                return makes.Data;
            }
            else
            {
                // TODO: logging
                throw new Exception("CarAPI call failed");
            }
        }

        public async Task<List<CarAPIModel>> GetModels(string year, string make)
        {
            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{carAPIOptions.Value.BaseUrl}api/models/v2?year={year}&make={make}");

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var models = JsonSerializer.Deserialize<CarAPIModelsResponse>(jsonContent);
                return models.Data;
            }
            else
            {
                // TODO: logging
                throw new Exception("CarAPI call failed");
            }
        }

        public async Task<List<CarAPISubModel>> GetSubModels(string year, string make, string model)
        {
            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{carAPIOptions.Value.BaseUrl}api/submodels/v2?year={year}&make={make}&model={model}");

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var subModels = JsonSerializer.Deserialize<CarAPISubModelsResponse>(jsonContent);
                return subModels.Data;
            }
            else
            {
                // TODO: logging
                throw new Exception("CarAPI call failed");
            }
        }

        public async Task<CarAPIVINResponse?> GetVIN(string vin)
        {
            var httpClient = httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{carAPIOptions.Value.BaseUrl}api/vin/{vin}");

            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var vinResponse = JsonSerializer.Deserialize<CarAPIVINResponse>(jsonContent);
                return vinResponse;
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw new Exception("CarAPI call failed");
                }
            }
        }
    }
}
