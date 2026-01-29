using AutoMechanic.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoMechanic.Services.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<Guid> AddVehicleAsync(VehicleDTO vehicleDto);
        Task<List<VehicleDTO>> GetVehiclesByCustomerIdAsync(Guid customerId);
    }
}
