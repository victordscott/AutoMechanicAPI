using System;
using System.Collections.Generic;

namespace AutoMechanic.DataAccess.EF.Models;

public partial class AppointmentNote
{
    public Guid AppointmentNoteId { get; set; }

    public Guid AppointmentId { get; set; }

    public Guid UserId { get; set; }

    public string Note { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedDate { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
