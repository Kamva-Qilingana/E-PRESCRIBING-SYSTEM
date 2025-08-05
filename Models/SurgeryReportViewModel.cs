

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class SurgeryReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<SurgeryData> SurgeryList { get; set; }
        public int TotalPatients { get; set; }
        public Dictionary<string, int> TreatmentCodeSummary { get; set; }
    }

    public class SurgeryData
    {
        public DateTime SurgeryDate { get; set; }
        public string PatientName { get; set; }
        public List<string> TreatmentCodes { get; set; }
    }

}
