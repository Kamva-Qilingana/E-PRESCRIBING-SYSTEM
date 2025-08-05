using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class SuburbViewModel
    {
        public int SuburbID { get; set; }
        public string SuburbName { get; set; }
        public string PostalCode { get; set; }

        public int CityID { get; set; }
        public IEnumerable<SelectListItem> Cities { get; set; } // For dropdown list
    }
}
