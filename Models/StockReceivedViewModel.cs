using System.ComponentModel.DataAnnotations;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class StockReceivedViewModel
    {
        public int StockOrderID { get; set; }

        public int PharmacyMedicationId { get; set; }
        public string Name { get; set; }

        public int OrderQuantity { get; set; }

        [Required]
        public int ReceivedQuantity { get; set; }

    }
}
