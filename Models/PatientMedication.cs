using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class PatientMedication
    {
        [Key]
        public int PatientMedicationId { get; set; }

        // Foreign Key for PatientProfile
        [ForeignKey("PatientProfile")]
        public int PatientProfileId { get; set; }
        public virtual PatientProfile PatientProfile { get; set; }

        // Foreign Key for PharmacyMedication
        [ForeignKey("PharmacyMedication")]
        public int PharmacyMedicationId { get; set; }
        public virtual PharmacyMedication PharmacyMedication { get; set; }

        // Additional fields (e.g., quantity, instructions)
        public int Quantity { get; set; }
        public string Instructions { get; set; }
    }
}
