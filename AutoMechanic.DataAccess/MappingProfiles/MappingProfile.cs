using AutoMapper;
using AutoMechanic.DataAccess.DTO;
using AutoMechanic.DataAccess.EF.Models;
using AutoMechanic.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            JsonSerializerOptions jso = null;

            CreateMap<CreateConsultantRequest, ApplicationUser>();
            CreateMap<ConsultantAvailabilityScheduleDTO, ConsultantAvailabilitySchedule>()
                .ForMember(x => x.RecurrenceExceptions, opt => opt.MapFrom(src => 
                    src.RecurrenceExceptions != null && src.RecurrenceExceptions.Count > 0
                        ? JsonSerializer.Serialize(src.RecurrenceExceptions, src.RecurrenceExceptions!.GetType(), jso)
                        : null)
                );
            CreateMap<ConsultantAvailabilitySchedule, ConsultantAvailabilityScheduleDTO>()
                .ForMember(x => x.RecurrenceExceptions, opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.RecurrenceExceptions)
                        ? null // new List<string>()
                        : JsonSerializer.Deserialize<List<string>>(src.RecurrenceExceptions, jso)
                    )
                );
            CreateMap<ConsultantAvailabilityDateDTO, ConsultantAvailabilityDate>()
                .ForMember(x => x.ConsultantAvailabilityDateId, expression => expression.Ignore());
            CreateMap<EF.Models.SupportedTimeZone, TimeZoneDTO>();
            CreateMap<VehicleDTO, Vehicle>();
        }
    }
}
