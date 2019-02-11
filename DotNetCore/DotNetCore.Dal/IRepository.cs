using DotNetCore.Infrastruct.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Dal
{
   public interface IRepository<TEntity>: IRepository<TEntity,int> where TEntity:class,IEntity<int>
    {
    }
}
