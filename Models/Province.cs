using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.ComponentModel;
using E_PRESCRIBING_SYSTEM.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class Province
    {
        [Key]
        public int ProvinceID { get; set; }
        [Required]
        public string ProvinceName { get; set; }
        public virtual ICollection<City> Citys
        {
            get; set;
        }
    }
}
