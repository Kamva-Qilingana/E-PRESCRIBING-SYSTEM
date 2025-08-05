namespace E_PRESCRIBING_SYSTEM.Models
{
    public class ReceiveStockViewModel
    {
        public int StockID { get; set; } // Order Stock ID
        public int PharmacyMedicationId { get; set; } // Medication ID
        public int ReceivedQuantity { get; set; } // Quantity to receive
    }
}
