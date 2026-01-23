using AutoMechanic.DataAccess.DTO;
using AutoMechanic.DataAccess.EF.Models;
using AutoMechanic.DataAccess.Models;
using Ical.Net.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.Services.Services.Interfaces
{
    public interface IConsultantService
    {
        Task<ConsultantInfo> GetConsultantInfoAsync(Guid userId);
        Task<List<ConsultantAvailabilityScheduleDTO>> GetSchedulesByUserIdAsync(Guid userId);
        Task<bool> DeleteScheduleAsync(Guid scheduleId);
        Task<ConsultantAvailabilityScheduleDTO> UpdateScheduleAsync(ConsultantAvailabilityScheduleDTO schedule);
        Task<ConsultantAvailabilityScheduleDTO> InsertScheduleAsync(ConsultantAvailabilityScheduleDTO schedule);
        Task<List<ConsultantAvailabilityScheduleDTO>> SaveScheduleChangesAsync(
            Guid userId,
            ScheduleChanges scheduleChanges
        );
        Task<List<Period>> ExpandSchedules(Guid userId);
        Task<List<Period>> ExpandSchedules();
        Task<bool> UpdateAvailabilityDates();
    }
}
