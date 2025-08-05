using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace E_PRESCRIBING_SYSTEM.Models
{
    public class PrescriptionMedication
    {
        [Key]
        public int PrescriptionMedicationId { get; set; }

        [ForeignKey("NewPrescription")]
        public int PrescriptionId { get; set; }
        public virtual NewPrescription NewPrescription { get; set; }

        [ForeignKey("PharmacyMedication")]
        public int PharmacyMedicationId { get; set; }
        public virtual PharmacyMedication PharmacyMedication { get; set; }

        [Required]
        public int Quantity { get; set; }
        public string Instruction { get; set; }
    }

}
