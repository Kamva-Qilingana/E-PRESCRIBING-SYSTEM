namespace E_PRESCRIBING_SYSTEM.Models
{
    public class ChronicMedsViewModel
    {
        public int ChronicMedicationId { get; set; }
        public string ChronicMedicationName { get; set; }
        public int Schedule { get; set; }
        public string DosageFormName { get; set; }
        public List<string> ActiveIngredients { get; set; }
    }
}
