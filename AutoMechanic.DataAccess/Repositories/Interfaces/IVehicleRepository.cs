using AutoMechanic.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.Repositories.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Guid> AddVehicleAsync(VehicleDTO vehicleDto);
        Task<List<VehicleDTO>> GetVehiclesByCustomerIdAsync(Guid customerId);
    }
}
