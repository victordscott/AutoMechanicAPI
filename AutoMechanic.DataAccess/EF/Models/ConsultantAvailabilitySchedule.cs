using System;
using System.Collections.Generic;

namespace AutoMechanic.DataAccess.EF.Models;

public partial class ConsultantAvailabilitySchedule
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

    public string? RecurrenceExceptions { get; set; }

    public DateTime DateInserted { get; set; }

    public DateTime DateUpdated { get; set; }

    public virtual ICollection<ConsultantAvailabilityDate> ConsultantAvailabilityDates { get; set; } = new List<ConsultantAvailabilityDate>();

    public virtual AspNetUser User { get; set; } = null!;
}
