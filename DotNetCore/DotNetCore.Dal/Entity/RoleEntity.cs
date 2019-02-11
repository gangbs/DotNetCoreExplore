using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DotNetCore.Dal.Entity
{
    [Table(RoleEntity.TableName)]
    public class RoleEntity : BaseEntity
    {
        [NotMapped]
        public const string TableName = "Role";

        [Required]
        [StringLength(20)]
        public string Name { get; set; }
    }
}
