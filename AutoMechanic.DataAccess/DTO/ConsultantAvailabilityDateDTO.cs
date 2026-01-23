using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.DTO
{
    public class ConsultantAvailabilityDateDTO
    {
        public Guid ConsultantAvailabilityDateId { get; set; }
        public Guid ConsultantAvailabilityScheduleId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateInserted { get; set; }
    }
}
