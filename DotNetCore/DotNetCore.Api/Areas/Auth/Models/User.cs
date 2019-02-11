using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Api.Areas.Auth.Models
{
    public class User
    {
        [StringLength(maximumLength: 10, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public DateTime Birth { get; set; }
        [Phone]
        public string Phone { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
