using AutoMechanic.DataAccess.DTO;
using AutoMechanic.DataAccess.Models.Proc;
using AutoMechanic.DataAccess.Repositories.Interfaces;
using AutoMechanic.Services.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace AutoMechanic.Services.Services
{
    public class VehicleService(IVehicleRepository vehicleRepository) : IVehicleService
    {
        public async Task<Guid> AddVehicleAsync(VehicleDTO vehicleDTO)
        {
            return await vehicleRepository.AddVehicleAsync(vehicleDTO);
        }

        public async Task<VehicleWithFiles> AddVehicleWithFilesAsync(VehicleWithFiles vehicleWithFiles)
        {
            return await vehicleRepository.AddVehicleWithFilesAsync(vehicleWithFiles);
        }

        public async Task<VehicleWithFiles> UpdateVehicleWithFilesAsync(VehicleWithFiles vehicleWithFiles)
        {
            return await vehicleRepository.UpdateVehicleWithFilesAsync(vehicleWithFiles);
        }

        public async Task<List<VehicleDTO>> GetVehiclesByCustomerIdAsync(Guid customerId)
        {
            return await vehicleRepository.GetVehiclesByCustomerIdAsync(customerId);
        }

        public async Task<VehicleWithFiles> GetVehicleWithFilesAsync(Guid vehicleId)
        {
            return await vehicleRepository.GetVehicleWithFiles(vehicleId);
        }

        public async Task<bool> DeleteVehicleAsync(Guid vehicleId)
        {
            return await vehicleRepository.DeleteVehicleAsync(vehicleId);
        }
    }
}
