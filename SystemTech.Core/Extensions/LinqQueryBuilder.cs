using System.Linq.Expressions;

namespace SystemTech.Core.Extensions
{
    public static class LinqQueryBuilder
    {
        public static Expression<TDelegate> AndAlso<TDelegate>(this Expression<TDelegate> left, Expression<TDelegate> right) 
        { 
            return Expression.Lambda<TDelegate>(Expression.AndAlso(left.Body, right.Body), left.Parameters); 
        }
        
        public static Expression<TDelegate> And<TDelegate>(this Expression<TDelegate> left, Expression<TDelegate> right) 
        { 
            return Expression.Lambda<TDelegate>(Expression.Add(left.Body, right.Body), left.Parameters); 
        } 
    }
}

