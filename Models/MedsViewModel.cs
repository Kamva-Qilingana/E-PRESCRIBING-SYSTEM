namespace E_PRESCRIBING_SYSTEM.Models
{
    public class MedsViewModel
    {
        public int PharmacyMedicationId { get; set; }
        public string Name { get; set; }
        public int Schedule { get; set; }
        public int ReorderLevel { get; set; }
        public int StockOnHand { get; set; }
        public string DosageFormName { get; set; }
        public List<string> ActiveIngredients { get; set; } // List of active ingredients with their strengths
    }
}
