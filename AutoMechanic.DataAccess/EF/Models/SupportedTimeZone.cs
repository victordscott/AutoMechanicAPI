using System;
using System.Collections.Generic;

namespace AutoMechanic.DataAccess.EF.Models;

public partial class SupportedTimeZone
{
    public string TimeZoneAbbrev { get; set; } = null!;

    public string TimeZoneName { get; set; } = null!;

    public string TimeZoneIana { get; set; } = null!;

    public virtual ICollection<AspNetUser> AspNetUsers { get; set; } = new List<AspNetUser>();
}
