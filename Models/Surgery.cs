using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class Surgery
    {
        [Key]
        public int SurgeryID { get; set; }

        [Required]
        public int PatientProfileId { get; set; }
        [ForeignKey("PatientProfileId")]
        public virtual PatientProfile PatientProfile { get; set; }

        [Required]
        public int TheatreID { get; set; }
        [ForeignKey("TheatreID")]
        public virtual Threatre Theatre { get; set; }

        [Required(ErrorMessage = "Surgery Date is required.")]
        [DataType(DataType.Date)]
        public DateTime SurgeryDate { get; set; }

        [Required(ErrorMessage = "Time Slot is required.")]
        public string TimeSlot { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property for multiple TreatmentCodes
        public virtual ICollection<SurgeryTreatment> SurgeryTreatments { get; set; }


    }


}
