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
    public class ActiveMedicationIngredient
    {
        [Key]
        public int ActiveMedicationId { get; set; }

        [ForeignKey("PharmacyMedicationId")]
        public int PharmacyMedicationId { get; set; }
        public virtual PharmacyMedication PharmacyMedication { get; set; }

        [ForeignKey("ActiveIngredientsID")]
        public int ActiveIngredientsID { get; set; }
        public virtual ActiveIngredients ActiveIngredients { get; set; }

        //[ForeignKey("ChronicMedicationId")]
        //public int ChronicMedicationId { get; set; }
        //public virtual ChronicMedication ChronicMedication { get; set; }

        public string Strength { get; set; } // Store strength along with ActiveIngredient
    }
}
