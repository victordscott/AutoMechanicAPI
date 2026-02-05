using AutoMechanic.DataAccess.DTO;
using AutoMechanic.DataAccess.Models.Proc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Guid> AddVehicleAsync(VehicleDTO vehicleDTO);
        Task<VehicleWithFiles> AddVehicleWithFilesAsync(VehicleWithFiles vehicleWithFiles);
        Task<List<VehicleDTO>> GetVehiclesByCustomerIdAsync(Guid customerId);
        Task<VehicleWithFiles> GetVehicleWithFiles(Guid vehicleId);
    }
}
