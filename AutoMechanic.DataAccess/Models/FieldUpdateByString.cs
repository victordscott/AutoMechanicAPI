using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.Models
{
    public class FieldUpdateByString
    {
        public required string FieldName { get; set; }
        public object? Value { get; set; } 
    }
}
