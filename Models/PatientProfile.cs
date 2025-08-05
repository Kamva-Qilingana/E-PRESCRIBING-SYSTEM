using System;
using System.ComponentModel.DataAnnotations;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class PatientProfile
    {
        [Key]
        public int PatientProfileId { get; set; }

        [Required(ErrorMessage = "Patient ID number is required.")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Patient ID number must be exactly 13 characters.")]
        public string PatientIDno { get; set; }

        [Required(ErrorMessage = "Patient name is required.")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        public string PSurname { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string PAddress { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [Phone(ErrorMessage = "Invalid contact number.")]
        public string PContactNo { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        [Required]
        public string Gender { get; set; }


        public virtual ICollection<NewPrescription> Prescriptions { get; set; }
        public virtual ICollection<PatientMedication> PatientMedications { get; set; }
        //public virtual ICollection<PatientAllergy> Allergies { get; set; }

        // Relationships
        public virtual ICollection<PatientCondition> PatientConditions { get; set; }
       // public virtual ICollection<PatientMedication> PatientMedications { get; set; }
        public virtual ICollection<PatientAllergy> PatientAllergies { get; set; }
    }
}
