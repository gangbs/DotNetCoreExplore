using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace DotNetCore.Dal.Ado
{
   public class DbConnectionFactory
    {
        public static DbConnection GetDbConnection()
        {
            DbConnection dbConnect = null;
            switch (DbConfig.Instance.DbType)
            {
                case DatabaseType.SqlServer:
                    dbConnect = new SqlConnection(DbConfig.Instance.ConnectionStrings);
                    break;
                case DatabaseType.MySql:
                    //dbConnect=
                    break;
                case DatabaseType.Oracle:
                    //dbConnect=
                    break;
                default:
                    //dbConnect=
                    break;
            }
            return dbConnect;
        }
    }
}
