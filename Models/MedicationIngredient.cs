namespace E_PRESCRIBING_SYSTEM.Models
{
    public class MedicationIngredient
    {
            public int MedicationIngredientId { get; set; }
            public int MedicationId { get; set; }
            public Medication Medication { get; set; }
            public int ActiveIngredientsID { get; set; }
            public ActiveIngredients ActiveIngredients { get; set; }
            public string Strength { get; set; }
        
    }
}
