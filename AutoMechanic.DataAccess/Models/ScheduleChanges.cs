using AutoMechanic.DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.Models
{
    public class ScheduleChanges
    {
        public List<ConsultantAvailabilityScheduleDTO> Created { get; set; }
        public List<ConsultantAvailabilityScheduleDTO> Updated { get; set; }
        public List<ConsultantAvailabilityScheduleDTO> Deleted { get; set; }
    }
}
