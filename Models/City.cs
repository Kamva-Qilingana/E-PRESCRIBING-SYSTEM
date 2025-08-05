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
    public class City
    {
        [Key]
        public int CityID { get; set; }
        [Required]
        public string CityName { get; set; }
        // Foreign key
        public int ProvinceID { get; set; }

        // Navigation property
        public virtual Province Province { get; set; }
        // Navigation Property
        public ICollection<Suburb> Suburbs { get; set; }
    }
}
