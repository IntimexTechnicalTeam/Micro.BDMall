using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

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

        public static int IntFromSql(this DatabaseFacade facade, string sql, params object[] parameters)
        {
            int count = 0;
            var command = CreateCommand(facade, sql, out DbConnection conn, parameters);
            var result = command.ExecuteScalar();
            if (result != null) count = int.Parse(result.ToString());
            conn.Close();
            return count;
        }

    }
}
