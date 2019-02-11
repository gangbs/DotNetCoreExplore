using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DotNetCore.Dal.Entity
{
    [Table(UserEnity.TableName)]
    public class UserEnity : BaseEntity
    {
        [NotMapped]
        public const string TableName= "User";

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        public int Sex { get; set; }

        public int Age { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [StringLength(20)]
        public string Password { get; set; }
    }
}
