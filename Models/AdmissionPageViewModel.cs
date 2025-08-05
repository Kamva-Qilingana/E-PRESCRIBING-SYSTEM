namespace E_PRESCRIBING_SYSTEM.Models
{
    public class AdmissionPageViewModel
    {
        public IEnumerable<Admission> Admissions { get; set; }
        public Admission NewAdmission { get; set; } = new Admission();
    }
}
