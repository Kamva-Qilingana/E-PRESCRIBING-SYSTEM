using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class CityViewModel
    {
            public int CityID { get; set; }
            public string CityName { get; set; }
            public int ProvinceID { get; set; }
        public string ProvinceName { get; set; }
            public IEnumerable<SelectListItem> Provinces { get; set; }
        // Add this to store the list of cities
        public List<City> Citys { get; set; }

    }
}
