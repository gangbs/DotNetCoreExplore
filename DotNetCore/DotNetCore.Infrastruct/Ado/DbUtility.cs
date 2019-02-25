using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace DotNetCore.Infrastruct.Ado
{
   public class DbUtility
    {
        //public static DbConnection CreateConnection(string providerName, string strConn)
        //{
        //    DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);
        //    DbConnection conn = factory.CreateConnection();
        //    conn.ConnectionString = strConn;
        //    return conn;
        //}


        public static DbConnection CreateConnection(DbProviderFactory factory, string strConn)
        {
            DbConnection conn = factory.CreateConnection();
            conn.ConnectionString = strConn;
            return conn;
        }


        //public static IEnumerable<T> Query<T>(string sql)
        //{
        //    //DataAdapter da = new DataAdapter()
        //    //da.Fill(dt);
        //    DbDataReader
        //    DbCommand command=new DbCommand()
        //}

    }
}
