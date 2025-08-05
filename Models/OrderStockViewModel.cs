namespace E_PRESCRIBING_SYSTEM.Models
{
    public class OrderStockViewModel
    {
        public int StockOrderId { get; set; }
        public string Name { get; set; }
        public int OrderQuantity { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}
