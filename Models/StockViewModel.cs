namespace E_PRESCRIBING_SYSTEM.Models
{
    public class StockViewModel
    {

        public List<PharmacyMedicationViewModel> Medications { get; set; } = new List<PharmacyMedicationViewModel>();
        public List<OrderStockViewModel> StockOrders { get; set; } = new List<OrderStockViewModel>();

       public List<StockOrderViewModel> StockOrder { get; set; } // Add this if missing
        

    }
}
