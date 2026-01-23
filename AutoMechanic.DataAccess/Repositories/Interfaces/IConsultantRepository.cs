using AutoMechanic.DataAccess.DTO;
using AutoMechanic.DataAccess.EF.Models;
using AutoMechanic.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.Repositories.Interfaces
{
    public interface IConsultantRepository
    {
        Task<ConsultantInfo> GetConsultantInfoAsync(Guid userId);
        Task<List<ConsultantAvailabilitySchedule>> GetAllSchedulesAsync();
        Task<List<ConsultantAvailabilitySchedule>> GetSchedulesByUserIdAsync(Guid userId);
        Task<bool> DeleteScheduleAsync(Guid scheduleId);
        Task<ConsultantAvailabilitySchedule> UpdateScheduleAsync(ConsultantAvailabilityScheduleDTO schedule);
        Task<ConsultantAvailabilitySchedule> InsertScheduleAsync(ConsultantAvailabilityScheduleDTO schedule);
        Task<List<ConsultantAvailabilitySchedule>> SaveScheduleChangesAsync(
            Guid userId,
            ScheduleChanges scheduleChanges
        );
        Task<int> DeleteAvailabilityDatesByUserIdAsync(Guid userId);
        Task<bool> InsertAvailabilityDatesAsync(List<ConsultantAvailabilityDateDTO> availabilityDates);
    }
}
