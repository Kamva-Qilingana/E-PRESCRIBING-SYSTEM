using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.ComponentModel;
using E_PRESCRIBING_SYSTEM.Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace E_PRESCRIBING_SYSTEM.Models
{
  
        public class Medication
        {
        [Key]
        public int MedicationID { get; set; }
        public string MedicationName { get; set; }
        public int ActiveIngredientsID { get; set; }
       
        public int DosageFormID { get; set; }
        public string IngredientStrength { get; set; }
        public int ReorderLevel { get; set; }
        public int StockOnHand { get; set; }

        [ForeignKey("ActiveIngredientsID")]
        public virtual ActiveIngredients ActiveIngredients { get; set; }

        [ForeignKey("DosageFormID")]
        public virtual DosageForm DosageForm { get; set; }
    }
    
}
