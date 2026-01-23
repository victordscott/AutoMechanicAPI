using AutoMechanic.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AutoMechanic.DataAccess.Models
{
    public class FindOptionsSort<T>
    {
        public Expression<Func<T, object>>? SortExpression { get; set; }
        public SortOrderDirection SortDirection { get; set; } = SortOrderDirection.Ascending;
    }
}
