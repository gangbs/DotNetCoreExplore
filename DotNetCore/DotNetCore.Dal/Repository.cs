using DotNetCore.Infrastruct.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Dal
{
   public class Repository<TEntity> : Repository<TEntity,int>, IRepository<TEntity> where TEntity : class, IEntity<int>
    {
        public Repository(DbContext dbContext):base(dbContext)
        {

        }
    }
}
