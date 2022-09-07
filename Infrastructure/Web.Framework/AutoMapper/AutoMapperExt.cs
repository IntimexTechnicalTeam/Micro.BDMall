namespace Web.Framework
{
    public static class AutoMapperExt
    {
        public static T MapTo<T>(this object obj)
        {
            return null == obj ? default(T) : AutoMapperConfiguration.Mapper.Map<T>(obj);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

        public static TDestination MapToList<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource">数据源</typeparam>
        /// <typeparam name="TDestination">目标源</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            return AutoMapperConfiguration.Mapper.Map<List<TDestination>>(source);
        }
    }

}
