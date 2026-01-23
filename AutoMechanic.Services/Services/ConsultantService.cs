using AutoMapper;
using AutoMechanic.DataAccess.DTO;
using AutoMechanic.DataAccess.EF.Models;
using AutoMechanic.DataAccess.Models;
using AutoMechanic.DataAccess.Repositories.Interfaces;
using AutoMechanic.Services.Services.Interfaces;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoMechanic.Services.Services
{
    public class ConsultantService(
        IConsultantRepository consultantRepository,
        IUserService userService,
        IMapper mapper
    ) : IConsultantService
    {

        public async Task<ConsultantInfo> GetConsultantInfoAsync(Guid userId)
        {
            return await consultantRepository.GetConsultantInfoAsync(userId);
        }

        public async Task<bool> DeleteScheduleAsync(Guid scheduleId)
        {
            return await consultantRepository.DeleteScheduleAsync(scheduleId);
        }

        public async Task<List<ConsultantAvailabilityScheduleDTO>> GetAllSchedulesAsync()
        {
            var schedules = await consultantRepository.GetAllSchedulesAsync();
            return mapper.Map<List<ConsultantAvailabilityScheduleDTO>>(schedules);
        }

        public async Task<List<ConsultantAvailabilityScheduleDTO>> GetSchedulesByUserIdAsync(Guid userId)
        {
            var schedules = await consultantRepository.GetSchedulesByUserIdAsync(userId);
            return mapper.Map<List<ConsultantAvailabilityScheduleDTO>>(schedules);
        }

        public async Task<ConsultantAvailabilityScheduleDTO> InsertScheduleAsync(ConsultantAvailabilityScheduleDTO schedule)
        {
            var scheduleInserted = await consultantRepository.InsertScheduleAsync(schedule);
            return mapper.Map<ConsultantAvailabilityScheduleDTO>(scheduleInserted);
        }

        public async Task<ConsultantAvailabilityScheduleDTO> UpdateScheduleAsync(ConsultantAvailabilityScheduleDTO schedule)
        {
            var scheduleUpdated = await consultantRepository.UpdateScheduleAsync(schedule);
            return mapper.Map<ConsultantAvailabilityScheduleDTO>(scheduleUpdated);
        }

        public async Task<List<ConsultantAvailabilityScheduleDTO>> SaveScheduleChangesAsync(
            Guid userId,
            ScheduleChanges scheduleChanges
         )
        {
            var schedules = await consultantRepository.SaveScheduleChangesAsync(userId, scheduleChanges);
            return mapper.Map<List<ConsultantAvailabilityScheduleDTO>>(schedules);
        }

        public async Task<bool> UpdateAvailabilityDates()
        {
            var consultants = await userService.GetConsultantsAsync();
            var utcNow = DateTime.UtcNow;
            foreach (var consultant in consultants)
            {
                var dates = new List<ConsultantAvailabilityDateDTO>();

                var userId = consultant.UserId!.Value;
                var tzi = TimeZoneInfo.FindSystemTimeZoneById(consultant.TimeZoneName!);
                var deletedCount = await consultantRepository.DeleteAvailabilityDatesByUserIdAsync(userId);

                //https://github.com/ical-org/ical.net/wiki/Working-with-recurring-elements
                var schedules = await GetSchedulesByUserIdAsync(userId);

                foreach (var schedule in schedules)
                {
                    if (!string.IsNullOrEmpty(schedule.RecurrenceRule))
                    {
                        var recurrencePattern = new RecurrencePattern(schedule.RecurrenceRule);
                        var calendarEvent = new CalendarEvent
                        {
                            DtStart = new CalDateTime(schedule.StartTime),
                            DtEnd = new CalDateTime(schedule.EndTime),
                            RecurrenceRules = new List<RecurrencePattern> { recurrencePattern },
                        };


                        // KendoReact scheduler does appear to support ExceptionRules
                        // calendarEvent.ExceptionRules = new List<RecurrencePattern> { exceptionPattern };

                        //calendarEvent.ExceptionDates

                        if (schedule.RecurrenceExceptions != null && schedule.RecurrenceExceptions.Count > 0)
                        {
                            foreach (var exception in schedule.RecurrenceExceptions)
                            {
                                calendarEvent.ExceptionDates.Add(new CalDateTime(exception));
                            }
                        }

                        var calendar = new Ical.Net.Calendar();
                        calendar.Events.Add(calendarEvent);

                        //get the occurrences within a specified time period
                        //var startSearch = new CalDateTime(schedule.StartTime);
                        var startSearch = new CalDateTime(DateTime.UtcNow.AddDays(-1));
                        var endSearch = new CalDateTime(DateTime.UtcNow.AddDays(45));

                        var occurrences = calendar.GetOccurrences(startSearch).TakeWhileBefore(endSearch).ToList();

                        System.Diagnostics.Debug.WriteLine($"--------------- {schedule.RecurrenceRule} ------------");
                        foreach (var o in occurrences)
                        {
                            dates.Add(new ConsultantAvailabilityDateDTO
                            {
                                ConsultantAvailabilityScheduleId = schedule.ConsultantAvailabilityScheduleId,
                                UserId = userId,
                                StartDate = DateTime.SpecifyKind(o.Period.StartTime.Value, DateTimeKind.Utc), // TimeZoneInfo.ConvertTimeToUtc(o.Period.StartTime.Value, tzi),
                                EndDate = DateTime.SpecifyKind(o.Period.EffectiveEndTime!.Value, DateTimeKind.Utc), //TimeZoneInfo.ConvertTimeToUtc(o.Period.EffectiveEndTime!.Value, tzi),
                                DateInserted = utcNow,
                            });
                        }
                    }
                    else
                    {
                        dates.Add(new ConsultantAvailabilityDateDTO
                        {
                            ConsultantAvailabilityScheduleId = schedule.ConsultantAvailabilityScheduleId,
                            UserId = userId,
                            StartDate = DateTime.SpecifyKind(schedule.StartTime, DateTimeKind.Utc),
                            EndDate = DateTime.SpecifyKind(schedule.EndTime, DateTimeKind.Utc),
                            DateInserted = utcNow,
                        });
                    }
                }

                System.Diagnostics.Debug.WriteLine($"----------------- {consultant.UserName} ------------------");
                var datesSorted = dates.OrderBy(d => d.StartDate).ToList();
                foreach (var date in datesSorted)
                {
                    System.Diagnostics.Debug.WriteLine($"{date.StartDate} - {date.EndDate} - Kinds: {date.StartDate.Kind} - {date.EndDate.Kind}");
                }

                await consultantRepository.InsertAvailabilityDatesAsync(datesSorted);
            }

            return true;
        }

        public async Task<List<Period>> ExpandSchedules(Guid userId)
        {
            //https://github.com/ical-org/ical.net/wiki/Working-with-recurring-elements
            var schedules = await GetSchedulesByUserIdAsync(userId);
            var periods = new List<Period>();

            foreach (var schedule in schedules)
            {
                if (!string.IsNullOrEmpty(schedule.RecurrenceRule))
                {
                    var recurrencePattern = new RecurrencePattern(schedule.RecurrenceRule);
                    var calendarEvent = new CalendarEvent
                    {
                        DtStart = new CalDateTime(schedule.StartTime),
                        DtEnd = new CalDateTime(schedule.EndTime),
                        RecurrenceRules = new List<RecurrencePattern> { recurrencePattern },
                    };


                    // KendoReact scheduler does appear to support ExceptionRules
                    // calendarEvent.ExceptionRules = new List<RecurrencePattern> { exceptionPattern };

                    //calendarEvent.ExceptionDates

                    if (schedule.RecurrenceExceptions != null && schedule.RecurrenceExceptions.Count > 0)
                    {
                        foreach (var exception in schedule.RecurrenceExceptions)
                        {
                            calendarEvent.ExceptionDates.Add(new CalDateTime(exception));
                        }
                    }

                    var calendar = new Ical.Net.Calendar();
                    calendar.Events.Add(calendarEvent);

                    //get the occurrences within a specified time period
                    var startSearch = new CalDateTime(schedule.StartTime);
                    var endSearch = new CalDateTime(DateTime.Now.AddDays(45));
                    
                    var occurrences = calendar.GetOccurrences(startSearch).TakeWhileBefore(endSearch).ToList();

                    System.Diagnostics.Debug.WriteLine($"--------------- {schedule.RecurrenceRule} ------------");
                    foreach (var o in occurrences)
                    {
                        periods.Add(o.Period);
                        System.Diagnostics.Debug.WriteLine($"{o.Period.StartTime} - {o.Period.EndTime}");
                    }
                }
            }

            return periods;
        }

        public async Task<List<Period>> ExpandSchedules()
        {
            //https://github.com/ical-org/ical.net/wiki/Working-with-recurring-elements
            var schedules = await GetAllSchedulesAsync();
            var periods = new List<Period>();

            foreach (var schedule in schedules)
            {
                if (!string.IsNullOrEmpty(schedule.RecurrenceRule))
                {
                    var recurrencePattern = new RecurrencePattern(schedule.RecurrenceRule);
                    var calendarEvent = new CalendarEvent
                    {
                        DtStart = new CalDateTime(schedule.StartTime),
                        DtEnd = new CalDateTime(schedule.EndTime),
                        RecurrenceRules = new List<RecurrencePattern> { recurrencePattern },
                    };

                    // KendoReact scheduler does appear to support ExceptionRules
                    // calendarEvent.ExceptionRules = new List<RecurrencePattern> { exceptionPattern };

                    //calendarEvent.ExceptionDates
                    if (schedule.RecurrenceExceptions != null && schedule.RecurrenceExceptions.Count > 0)
                    {
                        foreach (var exception in schedule.RecurrenceExceptions)
                        {
                            calendarEvent.ExceptionDates.Add(new CalDateTime(DateTime.Parse(exception, null, System.Globalization.DateTimeStyles.RoundtripKind)));
                        }
                    }

                    var calendar = new Ical.Net.Calendar();
                    calendar.Events.Add(calendarEvent);

                    //get the occurrences within a specified time period
                    var startSearch = new CalDateTime(schedule.StartTime);
                    var endSearch = new CalDateTime(DateTime.UtcNow.AddDays(45));

                    var occurrences = calendar.GetOccurrences(startSearch).TakeWhileBefore(endSearch).ToList();

                    System.Diagnostics.Debug.WriteLine($"--------------- {schedule.RecurrenceRule} ------------");
                    foreach (var o in occurrences)
                    {
                        periods.Add(o.Period);
                        System.Diagnostics.Debug.WriteLine($"{o.Period.StartTime} - {o.Period.EndTime}");
                    }
                }
            }

            return periods;
        }
    }
}
