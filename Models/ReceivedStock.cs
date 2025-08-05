
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class ReceivedStock
    {
        [Key]
        public int ReceivedStockID { get; set; }

        public int ReceivedQuantity { get; set; }

        public DateTime ReceivedDate { get; set; }

        // Foreign Key for OrderStock with no cascading delete
        public int StockID { get; set; } // Change this line
        [ForeignKey("StockID")]
        public virtual OrderStock OrderStock { get; set; }

        // Foreign Key for PharmacyMedication with no cascading delete
        public int PharmacyMedicationId { get; set; } // Change this line
        [ForeignKey("PharmacyMedicationId")]
        public virtual PharmacyMedication PharmacyMedication { get; set; }
    }
}
