using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.Infrastruct.EF
{
    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}
