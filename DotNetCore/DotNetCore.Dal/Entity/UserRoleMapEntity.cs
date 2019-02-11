using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DotNetCore.Dal.Entity
{
    [Table(UserRoleMapEntity.TableName)]
    public class UserRoleMapEntity: BaseEntity
    {
        [NotMapped]
        public const string TableName = "UserRoleMap";

        public int UserId { get; set; }

        public int RoleId { get; set; }

        [ForeignKey(nameof(UserRoleMapEntity.UserId))]
        public virtual UserEnity User { get; set; }

        [ForeignKey(nameof(UserRoleMapEntity.RoleId))]
        public virtual RoleEntity Role { get; set; }
    }
}
