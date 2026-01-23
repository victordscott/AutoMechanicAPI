using System;
using System.Collections.Generic;

namespace AutoMechanic.DataAccess.EF.Models;

public partial class ConsultantAvailabilityDate
{
    public Guid ConsultantAvailabilityDateId { get; set; }

    public Guid ConsultantAvailabilityScheduleId { get; set; }

    public Guid UserId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime DateInserted { get; set; }

    public virtual ConsultantAvailabilitySchedule ConsultantAvailabilitySchedule { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
