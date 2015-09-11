using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Fzrain
{
    /// <summary>
    /// Queryable extensions
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// 查询对象的导航属性
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="queryable">Queryable</param>
        /// <param name="includeProperties">A list of properties to include</param>
        /// <returns>New queryable</returns>
        public static IQueryable<T> IncludeProperties<T>(this IQueryable<T> queryable,
            params Expression<Func<T, object>>[] includeProperties)
        {
            if (queryable == null)
                throw new ArgumentNullException(nameof(queryable));
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
                queryable = queryable.Include(includeProperty);
            return queryable;
        }

    }
}
