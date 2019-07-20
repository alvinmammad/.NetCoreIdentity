using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace Identity_example.Models
{
    public class City
    {
        public City()
        {
            Users = new HashSet<User>();
        }
        public int ID { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }

        public int CountryID { get; set; }
        public virtual Country Country{ get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}