using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.Models
{
    public class ApplicationUser : IdentityUser<Guid> { 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public short TimeZoneId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsActive { get; set; }
    }
}
