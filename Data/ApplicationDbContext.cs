using E_PRESCRIBING_SYSTEM.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_PRESCRIBING_SYSTEM.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DosageForm> Dosages { get; set; }
        public DbSet<ActiveIngredients> ActiveIngredients { get; set; }
        public DbSet<TreatmentCode> TreatmentCodes { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<Threatre> threatres { get; set; }
        public DbSet<Condition> conditions { get; set; }
        public DbSet<ContraIndication> ContraIndications { get; set; }
        public DbSet<GeneralMedication> GeneralMedication { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Citys { get; set; }
        public DbSet<Suburb> Suburbs { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<PharmacyMedication> PharmacyMedication { get; set; }
        // public DbSet<MedicationActiveIngredient> medicationActiveIngredientActive { get; set; }

        public DbSet<ActiveMedicationIngredient> ActiveMedicationIngredients { get; set; }
        public DbSet<OrderStock> OrderStock { get; set; }
     
        public DbSet<PatientProfile> PatientProfiles { get; set; }

        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<NewPrescription> NewPrescriptions { get; set; }

        public DbSet<PatientAllergy> patientAllergies { get; set; }
        public DbSet<PatientMedication> PatientMedications { get; set; }
        public DbSet<PatientCondition> PatientConditions { get; set; }
   
        public DbSet<VitalSign> VitalSigns { get; set; }
        public DbSet<StockOrder> StockOrders { get; set; }
        public DbSet<StockReceived> StockReceived { get; set; }

        public DbSet<Surgery> Surgery { get; set; }
        public DbSet<SurgeryTreatment> SurgeryTreatments { get; set; }

        //public DbSet<ChronicMedication> ChricMedication { get; set; }
        public DbSet<PrescriptionMedication> PrescriptionMedications { get; set; }
        public DbSet<Admission> Admissions { get; set; }


    }
}
