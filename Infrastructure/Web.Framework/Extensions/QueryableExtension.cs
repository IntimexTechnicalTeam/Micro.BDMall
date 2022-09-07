namespace Web.Framework
{
    public static class QueryableExtension
    {
        /// <summary>
        /// 暂时不支持匿名类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public static IQueryable<T> SortBy<T>(this IQueryable<T> target, string propertyName, SortType orderType = SortType.ASC)
        {

            string sortBy = orderType == SortType.ASC ? "OrderBy" : "OrderByDescending";

            ParameterExpression oParameter = Expression.Parameter(typeof(T), "o");
            var property = typeof(T).GetProperty(propertyName);
            var memberExpression = Expression.Property(oParameter, property);
            var orderExpression = Expression.Lambda(memberExpression, new ParameterExpression[] { oParameter });
            var resultExpression = Expression.Call(typeof(Queryable), sortBy, new Type[] { target.ElementType, property.PropertyType },
                                                    new Expression[] { target.Expression, Expression.Quote(orderExpression) });
            return target.Provider.CreateQuery<T>(resultExpression) as IQueryable<T>;

        }

    }

    public enum SortType
    {
        /// <summary>
        /// 正序
        /// </summary>
        ASC,
        /// <summary>
        /// 倒序
        /// </summary>
        DESC
    }
}
