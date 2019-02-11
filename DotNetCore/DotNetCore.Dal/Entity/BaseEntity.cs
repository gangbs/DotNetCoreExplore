using DotNetCore.Infrastruct.EF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DotNetCore.Dal.Entity
{
    public class BaseEntity:IEntity<int>
    {
        [Key]
        public int Id { get; set; }
    }

    //public class TableName
    //{
    //    public const
    //}
}
