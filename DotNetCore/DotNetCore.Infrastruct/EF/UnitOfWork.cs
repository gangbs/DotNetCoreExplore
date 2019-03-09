using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Infrastruct.EF
{
   public abstract class UnitOfWork<DatabaseContext, TPrimaryKey> :IUnitOfWork,IDisposable where DatabaseContext : DbContext
    {
        protected readonly DbContext _dbContext;

        public UnitOfWork(DbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public abstract TRepository GetRepository<TRepository, TEntity>() 
            where TRepository : class, IRepository<TEntity, TPrimaryKey> 
            where TEntity:class, IEntity<TPrimaryKey>;


        public SaveResult SaveChanges()
        {
            SaveResult r;
            try
            {
                int count = _dbContext.SaveChanges();
                r = new SaveResult { Success = true, Rows = count };
            }
            catch (Exception exp)
            {
                r = new SaveResult { Success = false, Message = exp.Message };
            }
            return r;
        }

        public void Dispose()
        {
            this._dbContext.Dispose();
        }
    }
}
