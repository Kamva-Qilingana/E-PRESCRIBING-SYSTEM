namespace E_PRESCRIBING_SYSTEM.Models
{
    public class NewPrescriptionViewModel
    {
        public int PatientProfileId { get; set; }
        public string Status { get; set; }
        public bool IsAgent { get; set; }
        public string Instruction { get; set; } 
        public DateTime DataAdded { get; set; }
        public List<MedicationDetail> Medications { get; set; }
    }

    public class MedicationDetail
    {
        public int PharmacyMedicationId { get; set; }
        public int Quantity { get; set; }
        public string Instruction { get; set; }
    }

}
