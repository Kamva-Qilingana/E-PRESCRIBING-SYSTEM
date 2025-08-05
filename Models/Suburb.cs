using System.ComponentModel.DataAnnotations;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class Suburb
    {
        [Key]
        public int SuburbID { get; set; }
        public string SuburbName { get; set; }
        public string PostalCode { get; set; }

        // Foreign Key to City
        public int CityID { get; set; }

        // Navigation Property
        public City City { get; set; }
    }

}
