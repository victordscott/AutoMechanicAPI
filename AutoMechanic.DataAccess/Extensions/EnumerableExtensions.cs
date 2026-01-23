using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMechanic.Common.Enums;
using AutoMechanic.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoMechanic.DataAccess.Extensions
{
    public static class EnumerableExtensions
    {
        public static IQueryable<T> ApplyOrderBy<T>(this IQueryable<T> query, List<FindOptionsSort<T>>? sorts)
        {
            if (sorts is null || sorts.Count == 0) return query;

            var sequence = 0;
            foreach (var sort in sorts)
            {
                var sortByField = GetSortField<T>(sort);
                query = query.ApplyOrderBy<T>(sortByField, sort.SortDirection, sequence);
                sequence++;
            }

            return query;
        }
        private static IQueryable<T> ApplyOrderBy<T>(this IQueryable<T> query, string? sortByField, SortOrderDirection sortOrderDirection, int sequence)
        {
            if (sortByField is null) return query;

            IOrderedQueryable<T>? orderedQuery = null;

            if (sequence == 0)
            {
                query = sortOrderDirection == SortOrderDirection.Ascending
                    ? query.OrderBy(p => Microsoft.EntityFrameworkCore.EF.Property<T>(p!, sortByField))
                    : query.OrderByDescending(p => Microsoft.EntityFrameworkCore.EF.Property<T>(p!, sortByField));
            }
            else
            {
                orderedQuery = (query as IOrderedQueryable<T>);
                orderedQuery = sortOrderDirection == SortOrderDirection.Ascending
                    ? orderedQuery.ThenBy(p => Microsoft.EntityFrameworkCore.EF.Property<T>(p!, sortByField))
                    : orderedQuery.ThenByDescending(p => Microsoft.EntityFrameworkCore.EF.Property<T>(p!, sortByField));
            }

            return (orderedQuery ?? query);
        }

        public static IQueryable<T> ApplyWhere<T>(this IQueryable<T> query, Expression<Func<T, bool>>? filter)
        {
            if (filter is null) return query;

            return query.Where(filter);
        }

        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int? skip, int? limit)
        {
            if (skip is null && limit is null)
                return query;

            if (skip is not null && limit is not null)
                return query.Skip(skip.Value).Take(limit.Value);
            else if (skip is not null)
                return query.Skip(skip.Value);
            else if (limit is not null)
                return query.Take(limit.Value);
            else
                return query;
        }

        private static string GetSortField<T>(FindOptionsSort<T> sort)
        {
            if (sort.SortExpression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }
            else if (sort.SortExpression.Body is UnaryExpression unaryExpression
                && unaryExpression.Operand is MemberExpression operandMemberExpression)
            {
                return operandMemberExpression.Member.Name;
            }
            else
            {
                throw new ArgumentException("Invalid property expression");
            }
        }

        //public static IQueryable<T> OrderByField<T>(this IQueryable<T> queryable, string? sortByField, SortOrderDirection sortOrderDirection)
        //{
        //    if (!string.IsNullOrEmpty(sortByField))
        //    {
        //        var elementType = typeof(T);
        //        var orderByMethodName = (sortOrderDirection == SortOrderDirection.Ascending) ? "OrderBy" : "OrderByDescending";

        //        var parameterExpression = Expression.Parameter(elementType);
        //        var propertyOrFieldExpression = Expression.PropertyOrField(parameterExpression, sortByField);
        //        var selector = Expression.Lambda(propertyOrFieldExpression, parameterExpression);

        //        var orderByExpression = Expression.Call(typeof(Queryable), orderByMethodName,
        //                                                new[] { elementType, propertyOrFieldExpression.Type }, queryable.Expression, selector);

        //        return queryable.Provider.CreateQuery<T>(orderByExpression);
        //    }
        //    else
        //    {
        //        return queryable;
        //    }
        //}
    }
}
