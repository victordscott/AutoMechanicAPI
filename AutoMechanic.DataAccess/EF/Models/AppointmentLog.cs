using System;
using System.Collections.Generic;

namespace AutoMechanic.DataAccess.EF.Models;

public partial class AppointmentLog
{
    public Guid AppointmentLogId { get; set; }

    public Guid AppointmentId { get; set; }

    public short AppointmentStatusId { get; set; }

    public string? Note { get; set; }

    public DateTime LogDate { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual AppointmentStatus AppointmentStatus { get; set; } = null!;
}
