using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.Common.Model
{
    public class BaseApiResponse
    {
        public bool Succeeded { get; set; } = true;
        public string Message { get; set; }
    }
}
