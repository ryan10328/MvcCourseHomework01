using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MvcHomework01
{
    public static class IQueryableExtensions
    {
        // from m$ forum
        // https://social.msdn.microsoft.com/Forums/en-US/8b85ed9a-f3f4-41a0-bc9f-02c868539746/dynamic-linq-orderby-using-string-names?forum=adodotnetentityframework
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string memberName)
        {
            ParameterExpression[] typeParams = { Expression.Parameter(typeof(T), "") };

            System.Reflection.PropertyInfo pi = typeof(T).GetProperty(memberName);

            return (IOrderedQueryable<T>)query.Provider.CreateQuery(
            Expression.Call(
             typeof(Queryable),
             "OrderBy",
             new Type[] { typeof(T), pi.PropertyType },
             query.Expression,
             Expression.Lambda(Expression.Property(typeParams[0], pi), typeParams)));
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string memberName)
        {
            ParameterExpression[] typeParams = { Expression.Parameter(typeof(T), "") };

            System.Reflection.PropertyInfo pi = typeof(T).GetProperty(memberName);

            return (IOrderedQueryable<T>)query.Provider.CreateQuery(
            Expression.Call(
             typeof(Queryable),
             "OrderByDescending",
             new Type[] { typeof(T), pi.PropertyType },
             query.Expression,
             Expression.Lambda(Expression.Property(typeParams[0], pi), typeParams)));
        }

    }
}