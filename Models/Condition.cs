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
    public class Condition
    {
        [Key]
        public int ConditionID { get; set; }
        [Required]
        public string Diagnosis { get; set; }

        // Navigation property for related ContraIndications
        public ICollection<ContraIndication> ContraIndications { get; set; }
    }
}
