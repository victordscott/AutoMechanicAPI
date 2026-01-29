using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.Configuration.Options
{
    public class CarAPIOptions
    {
        public const string CarAPI = "CarAPI";
        public string BaseUrl { get; set; } = string.Empty;
    }
}
