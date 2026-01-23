using AutoMechanic.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.Auth.Models
{
    public class AuthResponse : BaseApiResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
