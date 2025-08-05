using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class SurgeryTreatment
    {
        [Key]
        public int SurgeryTreatmentId { get; set; }
        public int SurgeryID { get; set; }
        [ForeignKey("SurgeryID")]
        public virtual Surgery Surgery { get; set; }

        public int TreatmentID { get; set; }
        [ForeignKey("TreatmentID")]
        public virtual TreatmentCode TreatmentCode { get; set; }
    }
}
