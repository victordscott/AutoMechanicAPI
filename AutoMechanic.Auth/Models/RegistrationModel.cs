using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.Auth.Models
{
    public class RegistrationModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string TimeZoneAbbrev { get; set; }
    }
}
