using E_PRESCRIBING_SYSTEM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class SurgeryViewModel
    {
        public int SurgeryID { get; set; }

        [Required]
        public int PatientProfileId { get; set; }
        public virtual PatientProfile PatientProfile { get; set; }

        [Required]
        public int TheatreID { get; set; }
        public virtual Threatre Theatre { get; set; }

        [Required(ErrorMessage = "Surgery Date is required.")]
        [DataType(DataType.Date)]
        public DateTime SurgeryDate { get; set; }

        [Required(ErrorMessage = "Time Slot is required.")]
        public string TimeSlot { get; set; }

        public string CreatedBy { get; set; }

        public List<PatientProfile> Patients { get; set; }
        public List<Threatre> Theatres { get; set; }
        public List<TreatmentCode> Treatments { get; set; }
        public List<SurgeryTreatment> SurgeryTreatments { get; set; }

        // List of selected treatment codes
    
        public List<int> SelectedTreatmentIDs { get; set; } = new List<int>();
    }

}
