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
        public bool UseFileShare { get; set; } = false;
        public string FileShareLocation { get; set; }
        public string UploadFolderName { get; set; }
    }
}
