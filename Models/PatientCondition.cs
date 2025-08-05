using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class PatientCondition
    {
        [Key]
        public int PatientConditionId { get; set; }

        // Foreign Key for PatientProfile
        [ForeignKey("PatientProfile")]
        public int PatientProfileId { get; set; }
        public virtual PatientProfile PatientProfile { get; set; }

        // Foreign Key for Condition
        [ForeignKey("Condition")]
        public int ConditionID { get; set; }
        public virtual Condition Condition { get; set; }

        public DateTime DiagnosisDate { get; set; } // Optional: date when the condition was diagnosed
    }
}
