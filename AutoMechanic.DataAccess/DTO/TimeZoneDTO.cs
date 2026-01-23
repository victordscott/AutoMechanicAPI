using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.DTO
{
    public class TimeZoneDTO
    {
        public short TimeZoneId { get; set; }

        public string TimeZoneName { get; set; } = null!;

        public string? TimeZoneAbbreviation { get; set; }

        public TimeSpan? UtcOffset { get; set; }

        public bool? IsDst { get; set; }
    }
}
