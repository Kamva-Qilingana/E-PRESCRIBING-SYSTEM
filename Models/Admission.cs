using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.ComponentModel;
using E_PRESCRIBING_SYSTEM.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_PRESCRIBING_SYSTEM.Models
{
    public class Admission
    {
        [Key]
        public int AdmissionId { get; set; }
        [ForeignKey("WardID")]
        public int WardID { get; set; }
        public virtual Ward Ward { get; set; }
        [ForeignKey("PatientProfileId")]
        public int PatientProfileId { get; set; }
        public virtual PatientProfile PatientProfile { get; set; }
        public string Username { get; set; }
        public DateTime Date {  get; set; }
        public double height { get; set; }
        public double weight { get; set; }
        public string status { get; set; }

        public virtual ICollection<Ward> Wards { get; set; }
        public virtual ICollection<PatientProfile> PatientProfiles { get; set; } = new List<PatientProfile>();
    }
}
