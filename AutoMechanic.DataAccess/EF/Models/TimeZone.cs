using System;
using System.Collections.Generic;

namespace AutoMechanic.DataAccess.EF.Models;

public partial class TimeZone
{
    public short TimeZoneId { get; set; }

    public string TimeZoneName { get; set; } = null!;

    public string? TimeZoneAbbreviation { get; set; }

    public TimeSpan? UtcOffset { get; set; }

    public bool? IsDst { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<AspNetUser> AspNetUsers { get; set; } = new List<AspNetUser>();
}
