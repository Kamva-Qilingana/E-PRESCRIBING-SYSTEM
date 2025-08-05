using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class NewPrescription
    {
       
        [Key]
        public int PrescriptionId { get; set; }

        [ForeignKey("PatientProfile")]
        public int PatientProfileId { get; set; }
        public virtual PatientProfile PatientProfile { get; set; }

        public DateTime DateAdded { get; set; }
        public string Username { get; set; }
        public string Status { get; set; }
        // New field to track if the prescription was done by an agent
        public bool IsAgent { get; set; }
        public string? Instruction { get; set; }
        public string? RejectionNote { get; set; }

        //[ForeignKey("PharmacyMedication")]
        //public int PharmacyMedicationId { get; set; }
        //public virtual PharmacyMedication PharmacyMedication { get; set; }
        public virtual ICollection<PharmacyMedication> PharmacyMedication { get; set; } = new List<PharmacyMedication>();

        // Allow multiple medications in a prescription
        public virtual ICollection<PrescriptionMedication> PrescriptionMedications { get; set; } = new List<PrescriptionMedication>();
    }
}
