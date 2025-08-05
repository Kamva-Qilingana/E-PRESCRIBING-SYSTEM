namespace E_PRESCRIBING_SYSTEM.Models
{
    public class ManagScriptsViewModel
    {
        public NewPrescription NewPrescription { get; set; }
        public ICollection<PatientCondition> PatientConditions { get; set; }
        public ICollection<PatientAllergy> PatientAllergies { get; set; }
    }
}
