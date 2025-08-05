using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class ActiveIngredientViewModel
    {
        //public int ActiveIngredientsID { get; set; }
        //public string IngredientStrength { get; set; }
        //public string ActiveIngredientsName { get; set; }


        public int ActiveIngredientsID { get; set; }
        public string ActiveIngredientsName { get; set; }
        public string IngredientStrength { get; set; }

    }
}
