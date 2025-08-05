using E_PRESCRIBING_SYSTEM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_PRESCRIBING_SYSTEM.ViewModels
{
    public class SurgerysViewModel
    {
        public int SurgeryID { get; set; }

        [Required]
        public int PatientProfileId { get; set; }

        [Required]
        public int TheatreID { get; set; }

        [Required]
        public int TreatmentID { get; set; }

        [Required(ErrorMessage = "Surgery Date is required.")]
        [DataType(DataType.Date)]
        public DateTime SurgeryDate { get; set; }

        [Required(ErrorMessage = "Time Slot is required.")]
        public string TimeSlot { get; set; }

        public string CreatedBy { get; set; }

        public List<PatientProfile> Patients { get; set; }
        public List<Threatre> Theatres { get; set; }
        public List<TreatmentCode> Treatments { get; set; }




        //public int PatientProfileId { get; set; }
        //public int TheatreID { get; set; }
        //public List<int> TreatmentID { get; set; } // Changed to List for multiple treatments
        //public DateTime SurgeryDate { get; set; }
        //public string TimeSlot { get; set; }

        //// Populate these from the database
        //public List<PatientProfile> Patients { get; set; }
        ////public List<Theatre> Theatres { get; set; }
        //public List<TreatmentCode> Treatments { get; set; }
    }
}
