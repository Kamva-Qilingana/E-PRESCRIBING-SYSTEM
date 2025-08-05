using System;
using System.Collections.Generic;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class PharmacistReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ReportGeneratedDate { get; set; }
        public int TotalScriptsDispensed { get; set; }
        public int TotalScriptsRejected { get; set; }
        public List<NewPrescription> Prescriptions { get; set; }
        public Dictionary<string, int> MedicationSummary { get; set; } // Medicine summary (Key: Medication name, Value: Qty Dispensed)
    }
}
