using System;
using System.Collections.Generic;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class PatientHistoryViewModel
    {
        // Patient Information
        public int PatientProfileId { get; set; }
        public string PatientIDno { get; set; }
        public string PatientName { get; set; }
        public DateTime? BirthDate { get; set; }

        // List of patient conditions (diagnoses)
        public List<PatientCondition> Conditions { get; set; }

        // List of patient medications
        public List<PatientMedication> Medications { get; set; }

        // List of patient allergies
        public List<PatientAllergy> Allergies { get; set; }
    }
}
