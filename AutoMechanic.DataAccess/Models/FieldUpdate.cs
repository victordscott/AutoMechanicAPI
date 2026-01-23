using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.Models
{
    public class FieldUpdate<T>
    {
        public required Expression<Func<T, object>> Expression { get; set; }
        public required object Value { get; set; } 
    }
}
