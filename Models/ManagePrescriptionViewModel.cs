using E_PRESCRIBING_SYSTEM.Models;

public class ManagePrescriptionViewModel
{
    public NewPrescription NewPrescription { get; set; }
    public ICollection<PatientCondition> PatientConditions { get; set; }
    public ICollection<PatientAllergy> PatientAllergies { get; set; }
    public int PrescriptionId { get; set; }
    public string PatientName { get; set; }
    public string Status { get; set; }
    public DateTime DateAdded { get; set; }
    public bool IsAgent { get; set; }
    public string Instruction { get; set; }
    public string RejectionNote { get; set; }
    public List<MedicationDetail> Medications { get; set; } = new List<MedicationDetail>();
}
public class MedicationDetail
{
    public string MedicationName { get; set; }
    public int Quantity { get; set; }
    public string DosageForm { get; set; }
    public string Instruction { get; set; }
}

