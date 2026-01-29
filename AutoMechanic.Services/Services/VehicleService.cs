using AutoMechanic.DataAccess.DTO;
using AutoMechanic.DataAccess.Repositories.Interfaces;
using AutoMechanic.Services.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace AutoMechanic.Services.Services
{
    public class VehicleService(IVehicleRepository vehicleRepository) : IVehicleService
    {
        public async Task<Guid> AddVehicleAsync(VehicleDTO vehicleDto)
        {
            return await vehicleRepository.AddVehicleAsync(vehicleDto);
        }

        public async Task<List<VehicleDTO>> GetVehiclesByCustomerIdAsync(Guid customerId)
        {
            return await vehicleRepository.GetVehiclesByCustomerIdAsync(customerId);
        }
    }
}
