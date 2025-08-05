using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class PatientAllergy
    {
        [Key]
        public int PatientAllergyId { get; set; }

        // Foreign Key for PatientProfile
        [ForeignKey("PatientProfile")]
        public int PatientProfileId { get; set; }
        public virtual PatientProfile PatientProfile { get; set; }

        // Foreign Key for ActiveIngredients
        [ForeignKey("ActiveIngredients")]
        public int ActiveIngredientsID { get; set; }
        public virtual ActiveIngredients ActiveIngredients { get; set; }

        [Required]
        public string AllergyDescription { get; set; } // A brief description of the allergy
    }
}
