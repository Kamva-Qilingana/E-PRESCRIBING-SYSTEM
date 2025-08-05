namespace E_PRESCRIBING_SYSTEM.Models
{
    public class PatientAllergyViewModel
    {
        public int PatientProfileId { get; set; }
        public string PatientIDno { get; set; }
        public string PatientName { get; set; }
        public string PSurname { get; set; }

        public string ActiveIngredientName { get; set; }

        public List<ActiveIngredients> ActiveIngredientsList { get; set; } // List of available active ingredients
        public List<int> SelectedActiveIngredients { get; set; } // Selected active ingredients by the patient

        public string AllergyDescription { get; set; } // Description of the allergy
    }
}
