using System;
using System.Collections.Generic;

namespace AutoMechanic.DataAccess.EF.Models;

public partial class AppointmentStatus
{
    public short AppointmentStatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public string? StatusDescription { get; set; }

    public virtual ICollection<AppointmentLog> AppointmentLogs { get; set; } = new List<AppointmentLog>();

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
