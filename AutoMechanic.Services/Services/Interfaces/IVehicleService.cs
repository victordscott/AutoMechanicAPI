using AutoMechanic.DataAccess.DTO;
using System;
using System.Threading.Tasks;

namespace AutoMechanic.Services.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<Guid> AddVehicleAsync(VehicleDTO vehicleDto);
    }
}
