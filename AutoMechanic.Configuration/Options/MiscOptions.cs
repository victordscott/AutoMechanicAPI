using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.Configuration.Options
{
    public class MiscOptions
    {
        public const string Misc = "Misc";
        public int OTPCodeExpireMinutes { get; set; } = 3;
    }
}
