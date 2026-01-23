using System;
using System.Collections.Generic;

namespace AutoMechanic.DataAccess.EF.Models;

public partial class ServiceLength
{
    public short ServiceLengthId { get; set; }

    public string ServiceLengthName { get; set; } = null!;

    public string ServiceLengthDesc { get; set; } = null!;

    public short LengthMinutes { get; set; }

    public decimal ServiceLengthCost { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
