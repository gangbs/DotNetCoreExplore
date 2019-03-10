using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Dal
{
   public class DbContextFactory
    {
        public static DbContext GetDbContext()
        {
            DbContext dbContext=null;
            switch (DbConfig.Instance.DbType)
            {
                case DatabaseType.SqlServer:
                    //dbContext=
                    break;
                case DatabaseType.MySql:
                    //dbContext=
                    break;
                case DatabaseType.Oracle:
                    //dbContext=
                    break;
                default:
                    //dbContext=
                    break;
            }
            return dbContext;
        }
    }
}
