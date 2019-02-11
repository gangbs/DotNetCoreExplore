using DotNetCore.Infrastruct.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Dal
{
    public class UnitOfWork : UnitOfWork<DbCoreContext,int>
    {
        private readonly Dictionary<string, Object> _dicRepository = new Dictionary<string, object>();


        public UnitOfWork(DbCoreContext dbContext):base(dbContext)
        {
        }


        public override IRepository<TEntity, int> GetRepository<TEntity>()
        {
            string typeName = typeof(TEntity).Name;
            object repository;
            if(!this._dicRepository.TryGetValue(typeName,out repository))
            {
                this._dicRepository[typeName] = new Repository<TEntity>(this._dbContext);
            }
            return (Repository<TEntity>)this._dicRepository[typeName];
        }
    }
}
