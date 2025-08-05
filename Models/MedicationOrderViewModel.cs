namespace E_PRESCRIBING_SYSTEM.Models
{
    public class MedicationOrderViewModel
    {

        public List<PharmacyMedicationViewModel> Medications { get; set; } = new List<PharmacyMedicationViewModel>();
        public List<StockOrderViewModel> StockOrders { get; set; } = new List<StockOrderViewModel>();
    }
}
