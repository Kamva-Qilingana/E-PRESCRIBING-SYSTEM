namespace E_PRESCRIBING_SYSTEM.Models
{
    public class ContraIndicationViewModel
    {
        public ContraIndication ContraIndication { get; set; }
        public List<ActiveIngredients> ActiveIngredients { get; set; }
        public List<Condition> Conditions { get; set; }
    }
}
