using AutoMapper;
using AutoMechanic.DataAccess.DirectAccess;
using AutoMechanic.DataAccess.DTO;
using AutoMechanic.DataAccess.EF.Context;
using AutoMechanic.DataAccess.EF.Models;
using AutoMechanic.DataAccess.Models.Proc;
using AutoMechanic.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.Repositories
{
    public class VehicleRepository(
        IDbContextFactory<AutoMechanicDbContext> dbContextFactory,
        IProcCaller procCaller,
        IMapper mapper
    ) : IVehicleRepository
    {
        public async Task<Guid> AddVehicleAsync(VehicleDTO vehicleDTO)
        {
            var vehicle = mapper.Map<Vehicle>(vehicleDTO);

            if (vehicle.VehicleId == Guid.Empty)
                vehicle.VehicleId = Guid.NewGuid();

            var now = DateTime.UtcNow;
            vehicle.DateCreated = now;
            vehicle.DateUpdated = now;

            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                await dbContext.Vehicles.AddAsync(vehicle);
                await dbContext.SaveChangesAsync();
            }

            return vehicle.VehicleId;
        }

        public async Task<List<VehicleDTO>> GetVehiclesByCustomerIdAsync(Guid customerId)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var vehicles = await dbContext.Vehicles
                    .Where(v => v.CustomerId == customerId && !v.IsDeleted)
                    .ToListAsync();
                return mapper.Map<List<VehicleDTO>>(vehicles);
            }
        }

        public async Task<VehicleFileDTO> AddFileUploadToVehicleAsync(VehicleFileDTO vehicleFileDTO)
        {
            var vehicleFile = mapper.Map<VehicleFile>(vehicleFileDTO);

            if (vehicleFile.VehicleId == Guid.Empty)
                vehicleFile.VehicleId = Guid.NewGuid();

            var now = DateTime.UtcNow;
            vehicleFile.DateCreated = now;
            vehicleFile.DateUpdated = now;

            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                await dbContext.VehicleFiles.AddAsync(vehicleFile);
                await dbContext.SaveChangesAsync();
            }

            return vehicleFileDTO;
        }

        public async Task<VehicleWithFiles> GetVehicleWithFiles(Guid vehicleId)
        {
            return await procCaller.CallProc<VehicleWithFiles>("get_vehicle_with_files", vehicleId);
        }
    }
}
