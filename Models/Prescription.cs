using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }

        // Foreign key for PatientProfile
        [ForeignKey("PatientProfile")]
        public int PatientProfileId { get; set; }
        public virtual PatientProfile PatientProfile { get; set; }

        // Foreign key for PharmacyMedication
        [ForeignKey("PharmacyMedication")]
        public int PharmacyMedicationId { get; set; }
        public virtual PharmacyMedication PharmacyMedication { get; set; }

        // Quantity of the prescribed medication
        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }

        // Date when the prescription was issued
        [Required(ErrorMessage = "Prescription date is required.")]
        public DateTime PrescriptionDate { get; set; }
    }
}
