using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.Configuration.Options
{
    public class JWTOptions
    {
        public const string JWT = "JWT";
        public int TokenValidMinutes { get; set; }
        public int RefreshTokenValidMinutes { get; set; }
        public string ValidIssuer { get; set; } = string.Empty;
        public string ValidAudience { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
    }
}
