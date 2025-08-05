namespace E_PRESCRIBING_SYSTEM.Models
{
    public class CombinedStockViewModel
    {
        public StockViewModel StockOrder { get; set; }
        public StockReceivedViewModel StockReceived { get; set; }

       // public List<StockOrderViewModel> StockOrders { get; set; }
        public List<PharmacyMedication> Medications { get; set; }
        public List<StockOrder> StockOrders { get; set; }
        public List<ReceivedStock> ReceivedStocks { get; set; }


        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
