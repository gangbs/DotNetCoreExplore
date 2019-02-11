using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Infrastruct.EF
{
   public interface IUnitOfWork
    {
        SaveResult SaveChanges();
    }
}
