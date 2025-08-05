using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;



namespace E_PRESCRIBING_SYSTEM.Models
{
    public class OrderStock
    {
        [Key]
        public int StockID { get; set; }
        public int OrderQuantity { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public int PharmacyMedicationId { get; set; }
        
        public PharmacyMedication PharmacyMedication { get; set; }
        //public virtual ICollection<ReceivedStock> ReceivedStocks { get; set; }
    }
}
