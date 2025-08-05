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
    public class GeneralMedication
    {
        [Key]
        public int GeneralMedicationID{ get; set; }
        public string MedicationName { get; set; }

        // Foreign Key for DosageForm
        public int DosageFormID { get; set; }
        public DosageForm DosageForm { get; set; }

        // Medication fields
        public string Schedules { get; set; }
        public int ReorderLevels { get; set; }
        public int StockOnHand { get; set; }
        public int strengths { get; set; }

        // Relationship with Active Ingredients (Many-to-Many)
       
    }
}
