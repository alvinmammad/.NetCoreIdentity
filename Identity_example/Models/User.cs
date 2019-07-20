using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity_example.Models
{
    public class User:IdentityUser
    {
        [Required, StringLength(100)]
        public string Firstname { get; set; }

        [StringLength(100)]
        public string Lastname { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        public int CityID { get; set; }

        public virtual City City { get; set; }
    }
}
