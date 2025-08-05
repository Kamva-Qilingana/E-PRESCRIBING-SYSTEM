using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class StockReceived
    {
        [Key]
        public int StockReceivedID { get; set; }

        public int ReceivedQuantity { get; set; }

        public DateTime ReceivedDate { get; set; }

        // Foreign Key linking to OrderStock
        [ForeignKey("StockOrderID")]
        public int StockOrderID { get; set; }
        public virtual StockOrder StockOrder { get; set; }

        // Foreign Key linking to PharmacyMedication
        [ForeignKey("PharmacyMedicationId")]
        public int PharmacyMedicationId { get; set; }
        public virtual PharmacyMedication PharmacyMedication { get; set; }
    }
}
