using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.DTO
{
    public class ConsultantAvailabilityScheduleDTO
    {
        public Guid ConsultantAvailabilityScheduleId { get; set; }

        public Guid UserId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime StartTime { get; set; }

        public string? StartTimeZone { get; set; }

        public DateTime EndTime { get; set; }

        public string? EndTimeZone { get; set; }

        public bool IsAllDay { get; set; }

        public Guid? RecurrenceId { get; set; }

        public string? RecurrenceRule { get; set; }

        public List<string>? RecurrenceExceptions { get; set; }
        public DateTime DateInserted { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
