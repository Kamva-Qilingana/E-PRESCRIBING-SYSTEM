using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class MedicationViewModel
    {
        //public string MedicationName { get; set; } = "";
        //public int ActiveIngredientsID { get; set; }
        //public IEnumerable<SelectListItem> ActiveIngredients { get; set; } = new List<SelectListItem>();
        //public int DosageFormID { get; set; }
        //public IEnumerable<SelectListItem> DosageForms { get; set; } = new List<SelectListItem>();
        //public string IngredientStrength { get; set; } = "";
        //public int ReorderLevel { get; set; }
        //public int StockOnHand { get; set; }



        //public string MedicationName { get; set; }
        //public int DosageFormID { get; set; }
        //public List<ActiveIngredientViewModel> ActiveIngredients { get; set; } = new List<ActiveIngredientViewModel>();
        //public int ReorderLevel { get; set; }
        //public int StockOnHand { get; set; }


        //public string MedicationName { get; set; }
        //public List<ActiveIngredientViewModel> ActiveIngredients { get; set; } = new List<ActiveIngredientViewModel>();
        //public int DosageFormID { get; set; }
        //public string ReorderLevel { get; set; }
        //public int StockOnHand { get; set; }



        public int MedicationID { get; set; }
        public string MedicationName { get; set; }
        public int StockOnHand { get; set; }
        public int ReorderLevel { get; set; }
        public int DosageFormID { get; set; }

        // List for holding multiple active ingredients and their strength
        public List<ActiveIngredientViewModel> ActiveIngredients { get; set; }




    }
}
