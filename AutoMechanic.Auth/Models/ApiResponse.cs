using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.Auth.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string SuccessMessage { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
