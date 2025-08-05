using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class VitalSign
    {
        [Key]
        public int VitalSignId { get; set; }

        // Foreign Key for PatientProfile
        [ForeignKey("PatientProfile")]
        public int PatientProfileId { get; set; }
        public virtual PatientProfile PatientProfile { get; set; }

        // User who recorded the vital signs
        public string Username { get; set; }

        // Vital sign properties
        [Required]
        public decimal BodyTemperature { get; set; }

        [Required]
        public int HeartRate { get; set; }

        [Required]
        public decimal BloodOxygenSaturation { get; set; }

        [Required]
        public decimal Height { get; set; }

        [Required]
        public decimal Weight { get; set; }

        public DateTime RecordedAt { get; set; } // Time when the vitals were taken
    }
}
