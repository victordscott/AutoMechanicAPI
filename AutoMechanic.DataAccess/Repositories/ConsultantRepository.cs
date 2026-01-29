using AutoMapper;
using AutoMechanic.DataAccess.DTO;
using AutoMechanic.DataAccess.EF.Context;
using AutoMechanic.DataAccess.EF.Models;
using AutoMechanic.DataAccess.Models;
using AutoMechanic.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.Repositories
{
    public class ConsultantRepository(
        IDbContextFactory<AutoMechanicDbContext> dbContextFactory, 
        IMapper mapper
    ) : IConsultantRepository
    {
        public async Task<ConsultantInfo> GetConsultantInfoAsync(Guid userId)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                return await dbContext.ConsultantInfos.Where(c => c.UserId == userId).FirstAsync();
            }
        }

        public async Task<List<ConsultantAvailabilitySchedule>> GetAllSchedulesAsync()
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                return await dbContext.ConsultantAvailabilitySchedules.ToListAsync();
            }
        }

        public async Task<bool> DeleteScheduleAsync(Guid scheduleId)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {


                var schedule = await dbContext.ConsultantAvailabilitySchedules.FirstOrDefaultAsync(s => s.ConsultantAvailabilityScheduleId == scheduleId);
                if (schedule is not null)
                {
                    var deletedCount = await dbContext.ConsultantAvailabilityDates.Where(d => d.ConsultantAvailabilityScheduleId == scheduleId).ExecuteDeleteAsync();
                    dbContext.ConsultantAvailabilitySchedules.Remove(schedule);
                    await dbContext.SaveChangesAsync();
                }
            }
            return true;
        }

        public async Task<List<ConsultantAvailabilitySchedule>> GetSchedulesByUserIdAsync(Guid userId)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                return await dbContext.ConsultantAvailabilitySchedules.Where(s => s.UserId == userId).ToListAsync();
            }
        }

        public async Task<ConsultantAvailabilitySchedule> InsertScheduleAsync(ConsultantAvailabilityScheduleDTO schedule)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var now = DateTime.UtcNow;
                var scheduleModel = mapper.Map<ConsultantAvailabilitySchedule>(schedule);
                schedule.DateUpdated = now;
                schedule.DateInserted = now;
                await dbContext.ConsultantAvailabilitySchedules.AddAsync(scheduleModel);
                await dbContext.SaveChangesAsync();
                return scheduleModel;
            }
        }

        public async Task<ConsultantAvailabilitySchedule> UpdateScheduleAsync(ConsultantAvailabilityScheduleDTO schedule)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                var scheduleModel = mapper.Map<ConsultantAvailabilitySchedule>(schedule);
                schedule.DateUpdated = DateTime.UtcNow;
                dbContext.Attach(scheduleModel);
                dbContext.Entry(scheduleModel).State = EntityState.Modified;
                //dbContext.Set<ConsultantAvailabilitySchedule>().Update(scheduleModel);
                await dbContext.SaveChangesAsync();
                return scheduleModel;
            }
        }
        
        public async Task<List<ConsultantAvailabilitySchedule>> SaveScheduleChangesAsync(
            Guid userId,
            ScheduleChanges scheduleChanges
        )
        {
            if (scheduleChanges.Deleted is not null && scheduleChanges.Deleted.Count > 0)
            {
                foreach (var s in scheduleChanges.Deleted)
                {
                    await DeleteScheduleAsync(s.ConsultantAvailabilityScheduleId);
                }
            }

            if (scheduleChanges.Updated is not null && scheduleChanges.Updated.Count > 0)
            {
                foreach (var s in scheduleChanges.Updated)
                {
                    var scheduleUpdated = await UpdateScheduleAsync(s);
                }
            }

            if (scheduleChanges.Created is not null && scheduleChanges.Created.Count > 0)
            {
                foreach (var s in scheduleChanges.Created)
                {
                    s.UserId = userId;
                    var scheduleInserted = await InsertScheduleAsync(s);
                }
            }

            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                return await dbContext.ConsultantAvailabilitySchedules.Where(s => s.UserId == userId).ToListAsync();
            }
        }

        public async Task<int> DeleteAvailabilityDatesByUserIdAsync(Guid userId)
        {
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                return await dbContext.ConsultantAvailabilityDates.Where(s => s.UserId == userId).ExecuteDeleteAsync();
            }
        }

        public async Task<bool> InsertAvailabilityDatesAsync(List<ConsultantAvailabilityDateDTO> availabilityDateDTOs)
        {
            List<ConsultantAvailabilityDate> dates = mapper.Map<List<ConsultantAvailabilityDate>>(availabilityDateDTOs);
            using (var dbContext = dbContextFactory.CreateDbContext())
            {
                await dbContext.ConsultantAvailabilityDates.AddRangeAsync(dates);
                await dbContext.SaveChangesAsync();
            }
            return true;
        }
    }
}
