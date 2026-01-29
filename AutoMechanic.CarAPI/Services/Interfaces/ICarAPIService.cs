using AutoMechanic.CarAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.CarAPI.Services.Interfaces
{
    public interface ICarAPIService
    {
        Task<List<int>> GetYears();
        Task<List<CarAPIMake>> GetMakes(string year);
        Task<List<CarAPIModel>> GetModels(string year, string make);
        Task<List<CarAPISubModel>> GetSubModels(string year, string make, string model);
        Task<CarAPIVINResponse?> GetVIN(string vin);
    }
}
