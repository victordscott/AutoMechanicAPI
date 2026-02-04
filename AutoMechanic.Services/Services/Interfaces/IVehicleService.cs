using AutoMechanic.DataAccess.DTO;
using AutoMechanic.DataAccess.Models.Proc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoMechanic.Services.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<Guid> AddVehicleAsync(VehicleDTO vehicleDto);
        Task<List<VehicleDTO>> GetVehiclesByCustomerIdAsync(Guid customerId);
        Task<VehicleWithFiles> GetVehicleWithFilesAsync(Guid vehicleId);
    }
}
