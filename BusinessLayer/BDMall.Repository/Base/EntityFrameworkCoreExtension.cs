namespace BDMall.Repository
{
    public static class EntityFrameworkCoreExtension
    {
        private static DbCommand CreateCommand(DatabaseFacade facade, string sql, out DbConnection connection, params object[] parameters)
        {
            var conn = facade.GetDbConnection();
            connection = conn;
            conn.Open();
            var cmd = conn.CreateCommand();
            if (facade.IsSqlServer())
            {
                cmd.Parameters.Clear();
                cmd.CommandText = sql;
                cmd.Parameters.AddRange(parameters);
            }
            return cmd;
        }

        private static DataTable SqlQuery(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            var command = CreateCommand(facade, sql, out DbConnection conn, parameters);
            var reader = command.ExecuteReader();
            var dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            conn.Close();
            return dt;
        }

        /// <summary>
        /// 自定义Sql查询，并返回集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="facade"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<T> SqlQuery<T>(this DatabaseFacade facade, string sql, params object[] parameters) where T : class,new()
        {
            var dt = SqlQuery(facade, sql, parameters);
            return dt.DataTableToList<T>().ToList();
        }

        /// <summary>
        /// 返回查询语句条数
        /// </summary>
        /// <param name="facade"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int IntFromSql(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            int count = 0;
            var command = CreateCommand(facade, sql, out DbConnection conn, parameters);
            var result = command.ExecuteScalar();
            if (result != null) count = int.Parse(result.ToString());
            conn.Close();
            return count;
        }

        /// <summary>
        /// 异步返回查询语句条数
        /// </summary>
        /// <param name="facade"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static async  Task<int> IntFromSqlAsync(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            int count = 0;
            var command = CreateCommand(facade, sql, out DbConnection conn, parameters);
            var result = await command.ExecuteScalarAsync();
            if (result != null) count = int.Parse(result.ToString());
            conn.Close();
            return count;
        }
    }
}
