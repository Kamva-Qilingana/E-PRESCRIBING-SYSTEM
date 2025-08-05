namespace E_PRESCRIBING_SYSTEM.Models
{// ViewModels/PatientConditionViewModel.cs
    public class PatientConditionViewModel
    {
        public string PatientName { get; set; }
        public string ConditionName { get; set; }
        public string TreatmentCode { get; set; }
        public string Description { get; set; }
        public DateTime DateDiagnosed { get; set; }
    }
}
