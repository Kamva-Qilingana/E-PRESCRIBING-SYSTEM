namespace E_PRESCRIBING_SYSTEM.Models
{
    public class ReceivedViewModel
    {
        public int StockReceivedID { get; set; }

        public int StockOrderID { get; set; }

        public int PharmacyMedicationId { get; set; }

        public string MedicationName { get; set; }

        public int ReceivedQuantity { get; set; }

        public DateTime ReceivedDate { get; set; }

        public string StockOrderStatus { get; set; }
    }
}
