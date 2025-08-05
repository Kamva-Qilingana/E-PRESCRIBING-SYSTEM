using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class PharmacyMedicationViewModel
    {
        //public int MedicationId { get; set; }
        //public string Name { get; set; }
        //public int Schedule { get; set; }
        //public int ReorderLevel { get; set; }
        //public int StockOnHand { get; set; }

        //public int DosageFormID { get; set; }
        //public IEnumerable<SelectListItem> DosageForms { get; set; }

        //// For active ingredients and their strengths
        //public List<ActiveIngredientsViewModel> ActiveIngredients { get; set; }


        public int PharmacyMedicationId { get; set; }
        public string Name { get; set; }
        public int ReorderLevel { get; set; }
        public int OrderQuantity { get; set; }
        //public DateTime Created { get; set; }
        public int StockOnHand { get; set; }
        public bool IsSelected { get; set; }  // To track if the medication is selected for adding stock
    }
}
