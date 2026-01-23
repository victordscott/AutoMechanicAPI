using AutoMapper;
using AutoMechanic.Auth.Helpers;
using AutoMechanic.DataAccess.DTO;
using AutoMechanic.DataAccess.EF.Models;
using AutoMechanic.DataAccess.Models;
using AutoMechanic.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AutoMechanic.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ConsultantController(IConsultantService consultantService) : ControllerBase
    {
        [Authorize(Roles = "Consultant")]
        [HttpGet]
        public async Task<ConsultantInfo> GetConsultantInfo()
        {
            var userId = AuthHelper.GetUserIdFromPrincipal(User);

            return await consultantService.GetConsultantInfoAsync(userId);
        }

        [Authorize(Roles = "Consultant")]
        [HttpGet]
        public async Task<List<ConsultantAvailabilityScheduleDTO>> GetConsultantAvailabilitySchedules()
        {
            var userId = AuthHelper.GetUserIdFromPrincipal(User);
            var schedules = await consultantService.GetSchedulesByUserIdAsync(userId);

            foreach (var s in schedules)
            {
                if (s.RecurrenceExceptions is not null && s.RecurrenceExceptions.Count == 0)
                {
                    s.RecurrenceExceptions = null;
                }
            }

            return schedules;
        }

        [Authorize(Roles = "Consultant")]
        [HttpPut]
        public async Task<ConsultantAvailabilityScheduleDTO> UpdateConsultantAvailabilitySchedule(ConsultantAvailabilityScheduleDTO schedule)
        {
            var userId = AuthHelper.GetUserIdFromPrincipal(User);
            schedule.UserId = userId;
            var scheduleUpdated = await consultantService.UpdateScheduleAsync(schedule);

            return schedule;
        }

        [Authorize(Roles = "Consultant")]
        [HttpDelete]
        public async Task<IActionResult> DeleteConsultantAvailabilitySchedule(ConsultantAvailabilityScheduleDTO schedule)
        {
            await consultantService.DeleteScheduleAsync(schedule.ConsultantAvailabilityScheduleId);

            return Ok();
        }

        [Authorize(Roles = "Consultant")]
        [HttpPut]
        public async Task<List<ConsultantAvailabilityScheduleDTO>> SaveScheduleChanges(
            ScheduleChanges scheduleChanges
        )
        {
            var userId = AuthHelper.GetUserIdFromPrincipal(User);
            var schedules = await consultantService.SaveScheduleChangesAsync(userId, scheduleChanges);

            return schedules;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<dynamic> ExpandSchedules()
        {
            var schedules = await consultantService.ExpandSchedules();
            return schedules;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> UpdateAvailabilityDates()
        {
            var success = await consultantService.UpdateAvailabilityDates();
            return Ok();
        }
    }
}
