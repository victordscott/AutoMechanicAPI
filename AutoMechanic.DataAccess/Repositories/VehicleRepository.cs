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

        public async Task<VehicleWithFiles> AddVehicleWithFilesAsync(VehicleWithFiles vehicleWithFiles)
        {
            var vehicle = mapper.Map<Vehicle>(vehicleWithFiles);

            if (vehicle.VehicleId == Guid.Empty)
                vehicle.VehicleId = Guid.NewGuid();

            var now = DateTime.UtcNow;
            vehicle.DateCreated = now;
            vehicle.DateUpdated = now;

            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                await dbContext.Vehicles.AddAsync(vehicle);
                if (vehicleWithFiles.Files != null && vehicleWithFiles.Files.Count > 0)
                {
                    foreach (var file in vehicleWithFiles.Files)
                    {
                        await dbContext.VehicleFiles.AddAsync(new VehicleFile
                        {
                            VehicleFileId = Guid.NewGuid(),
                            VehicleId = vehicle.VehicleId,
                            FileUploadId = file.FileUploadId,
                            DateCreated = now,
                            DateUpdated = now
                        });
                    }
                }
                await dbContext.SaveChangesAsync();
            }

            return await GetVehicleWithFiles(vehicle.VehicleId);
        }

        public async Task<VehicleWithFiles> UpdateVehicleWithFilesAsync(VehicleWithFiles vehicleWithFiles)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var vehicle = await dbContext.Vehicles.FindAsync(vehicleWithFiles.VehicleId);

                if (vehicle != null)
                {
                    var mileageChanged = (vehicleWithFiles.CurrentMileage != vehicle.CurrentMileage);

                    var now = DateTime.UtcNow;
                    vehicle!.DateUpdated = now;
                    vehicle.CustomerNote = vehicleWithFiles.CustomerNote;
                    vehicle.CurrentMileage = vehicleWithFiles.CurrentMileage;

                    if (vehicleWithFiles.Files != null && vehicleWithFiles.Files.Count > 0)
                    {
                        foreach (var file in vehicleWithFiles.Files)
                        {
                            if (file.FrontEndState == Common.Enums.FrontEndState.Updated)
                            {
                                await dbContext.VehicleFiles
                                    .Where(f => f.VehicleFileId == file.EntityFileId)
                                    .ExecuteUpdateAsync(u => u
                                        .SetProperty(u => u.CustomerNote, file.CustomerNote)
                                        .SetProperty(u => u.DateUpdated, now)
                                );
                            }
                            else if (file.FrontEndState == Common.Enums.FrontEndState.Deleted)
                            {
                                await dbContext.VehicleFiles
                                    .Where(f => f.VehicleFileId == file.EntityFileId)
                                    .ExecuteUpdateAsync(u => u
                                        .SetProperty(u => u.IsDeleted, true)
                                        .SetProperty(u => u.DeletedDate, now)
                                );
                            }
                            else if (file.FrontEndState == Common.Enums.FrontEndState.Added)
                            {
                                await dbContext.VehicleFiles.AddAsync(new VehicleFile
                                {
                                    VehicleFileId = Guid.NewGuid(),
                                    VehicleId = vehicle.VehicleId,
                                    FileUploadId = file.FileUploadId,
                                    DateCreated = now,
                                    DateUpdated = now
                                });
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Vehicle not found.");
                }
                await dbContext.SaveChangesAsync();
            }

            return await GetVehicleWithFiles(vehicleWithFiles.VehicleId);
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

        public async Task<bool> DeleteVehicleAsync(Guid vehicleId)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                await dbContext.Vehicles
                    .Where(v => v.VehicleId == vehicleId)
                    .ExecuteUpdateAsync(u => u
                        .SetProperty(u => u.IsDeleted, true)
                        .SetProperty(u => u.DeletedDate, DateTime.UtcNow)
                    );
            }
            return true;
        }

        public async Task<VehicleWithFiles> GetVehicleWithFiles(Guid vehicleId)
        {
            return await procCaller.CallProc<VehicleWithFiles>("get_vehicle_with_files", vehicleId);
        }
    }
}
