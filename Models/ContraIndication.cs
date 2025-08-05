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
    public class ContraIndication
    {
        [Key]
        public int ContraIndicationId { get; set; }

        [ForeignKey("ActiveIngredients")]
        public int ActiveIngredientsID { get; set; }
        public virtual ActiveIngredients ActiveIngredients { get; set; }

        [ForeignKey("Condition")]
        public int ConditionID { get; set; }
        public virtual Condition Condition { get; set; }
    }

}
