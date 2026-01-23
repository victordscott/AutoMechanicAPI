using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.Configuration.Options
{
    public class DatabaseOptions
    {
        public const string Database = "Database";
        public string ConnectionString { get; set; } = string.Empty;
    }
}
