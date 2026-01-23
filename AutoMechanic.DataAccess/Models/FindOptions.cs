using AutoMechanic.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.Models
{
    public class FindOptions<T>
    {
        public List<FindOptionsSort<T>>? Sorts { get; set; }
        public int? Skip { get; set; }
        public int? Limit { get; set; }
        public bool RetrieveTotal { get; set; } = true;
    }
}
