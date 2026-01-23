using System;
using System.Collections.Generic;

namespace AutoMechanic.DataAccess.EF.Models;

public partial class ConsultantInfo
{
    public Guid? UserId { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public bool? EmailConfirmed { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public bool? PhoneNumberConfirmed { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public short? TimeZoneId { get; set; }

    public DateTime? DateCreated { get; set; }

    public bool? IsEnabled { get; set; }

    public bool? IsActive { get; set; }

    public Guid? RoleId { get; set; }

    public string? RoleName { get; set; }

    public string? TimeZoneName { get; set; }

    public string? TimeZoneAbbreviation { get; set; }

    public string? ConsultantDescription { get; set; }

    public Guid? PrimaryImageUploadId { get; set; }

    public Guid? PrimaryVideoUploadId { get; set; }

    public short? PrimaryImageFileTypeId { get; set; }

    public short? PrimaryVideoFileTypeId { get; set; }

    public string? PrimaryImageFileName { get; set; }

    public string? PrimaryVideoFileName { get; set; }

    public string? PrimaryImageFileNote { get; set; }

    public string? PrimaryVideoFileNote { get; set; }
}
