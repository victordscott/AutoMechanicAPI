using System;
using System.Collections.Generic;

namespace AutoMechanic.DataAccess.EF.Models;

public partial class AspNetUser
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string NormalizedUserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string NormalizedEmail { get; set; } = null!;

    public bool EmailConfirmed { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public string State { get; set; } = null!;

    public string Country { get; set; } = null!;

    public short TimeZoneId { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTime? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public DateTime DateCreated { get; set; }

    public bool IsEnabled { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Appointment> AppointmentConsultants { get; set; } = new List<Appointment>();

    public virtual ICollection<Appointment> AppointmentCustomers { get; set; } = new List<Appointment>();

    public virtual ICollection<AppointmentNote> AppointmentNotes { get; set; } = new List<AppointmentNote>();

    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();

    public virtual ICollection<ConsultantAvailabilityDate> ConsultantAvailabilityDates { get; set; } = new List<ConsultantAvailabilityDate>();

    public virtual ICollection<ConsultantAvailabilitySchedule> ConsultantAvailabilitySchedules { get; set; } = new List<ConsultantAvailabilitySchedule>();

    public virtual ConsultantDetail? ConsultantDetail { get; set; }

    public virtual ICollection<FileUpload> FileUploads { get; set; } = new List<FileUpload>();

    public virtual TimeZone TimeZone { get; set; } = null!;

    public virtual ICollection<UserFile> UserFiles { get; set; } = new List<UserFile>();

    public virtual ICollection<UserLoginOtpCode> UserLoginOtpCodes { get; set; } = new List<UserLoginOtpCode>();

    public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

    public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();
}
