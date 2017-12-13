using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DbService.Extension
{
    public static class LinqExtension
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string prop)
        {
            return OrderBy(source, prop, "OrderBy");
        }
        public static IQueryable<T> OrderByDescendingDynamic<T>(this IQueryable<T> source, string prop)
        {
            return OrderBy(source, prop, "OrderByDescending");
        }
        public static IQueryable<T> ThenByDynamic<T>(this IQueryable<T> source, string prop)
        {
            return OrderBy(source, prop, "ThenBy");
        }

        public static IQueryable<T> ThenByDescendingDynamic<T>(this IQueryable<T> source, string prop)
        {
            return /*(IOrderedQueryable<T>)*/OrderBy(source, prop, "ThenByDescending");
        }

        static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string prop, string orderString)
        {
            var type = typeof(T);
            var property = type.GetProperty(prop);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);           

            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), orderString, new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}
