using System;
using System.Collections.Generic;

namespace AutoMechanic.DataAccess.EF.Models;

public partial class Appointment
{
    public Guid AppointmentId { get; set; }

    public Guid ConsultantId { get; set; }

    public Guid CustomerId { get; set; }

    public short ServiceLengthId { get; set; }

    public short AppointmentStatusId { get; set; }

    public short? CustomerRatingId { get; set; }

    public bool ConsultantConfirmed { get; set; }

    public bool CustomerConfirmed { get; set; }

    public string? ConsultantNote { get; set; }

    public string? CustomerNote { get; set; }

    public short LengthMinutes { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime DateStatusChanged { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }

    public virtual ICollection<AppointmentFile> AppointmentFiles { get; set; } = new List<AppointmentFile>();

    public virtual ICollection<AppointmentLog> AppointmentLogs { get; set; } = new List<AppointmentLog>();

    public virtual ICollection<AppointmentNote> AppointmentNotes { get; set; } = new List<AppointmentNote>();

    public virtual AppointmentStatus AppointmentStatus { get; set; } = null!;

    public virtual AspNetUser Consultant { get; set; } = null!;

    public virtual AspNetUser Customer { get; set; } = null!;

    public virtual Rating? CustomerRating { get; set; }

    public virtual ServiceLength ServiceLength { get; set; } = null!;
}
