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
    public class PharmacyMedication
    {
        [Key]
        public int PharmacyMedicationId { get; set; }
        public string Name { get; set; }
        public int Schedule { get; set; }
        public int ReorderLevel { get; set; }
        public int StockOnHand { get; set; }

        // Foreign Key for DosageForm
        [ForeignKey("DosageFormID")]
        public int DosageFormID { get; set; }
        public virtual DosageForm DosageForm { get; set; }

        // Relationship to ActiveIngredients
        public virtual ICollection<ActiveIngredients> ActiveIngredients { get; set; }


        // Navigation property for ContraIndications
        public virtual ICollection<ContraIndication> ContraIndications { get; set; }
        // Navigation property for multiple Active Ingredients
        public virtual ICollection<ActiveMedicationIngredient> MedicationActiveIngredients { get; set; }
        // Navigation property
        // public virtual ICollection<ReceivedStock> ReceivedStocks { get; set; } // Add this line

        public virtual ICollection<Prescription> Prescriptions { get; set; }

        public virtual ICollection<NewPrescription> NewPrescriptions { get; set; }
        public virtual ICollection<PatientMedication> PatientMedications { get; set; }


    }
}
