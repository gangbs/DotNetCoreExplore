using DotNetCore.Dal.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;

namespace DotNetCore.Dal
{
    public class DbCoreContext : DbContext
    {
        public DbCoreContext() : base() { }

        readonly DbConnection _dbConnection;
        public DbCoreContext(DbConnection dbConnection) : base()
        {
            this._dbConnection = dbConnection;
        }

        ///// <summary>
        ///// 可在startup中注册，不太推荐
        ///// </summary>
        ///// <param name="options"></param>
        //public DbCoreContext(DbContextOptions<DbCoreContext> options) : base(options)
        //{

        //}

        /// <summary>
        /// 当使用无参构造函数时，使用当前方法去设置数据连接
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectStr = "server=localhost;port=3306;user=root;password=198603;database=DotNetCore";
            optionsBuilder.UseMySQL(connectStr);
        }


        public DbSet<UserEnity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserRoleMapEntity> UserRoleMaps { get; set; }

    }
}
