using E_PRESCRIBING_SYSTEM.Data;
using E_PRESCRIBING_SYSTEM.Models;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Text;

namespace E_PRESCRIBING_SYSTEM.Controllers
{
    public class SurgeonController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public SurgeonController(ApplicationDbContext dBD)
        {
            dbContext = dBD;
        }
        public IActionResult SurgeonDashboard()
        {
            var totalSurgeries = dbContext.Surgery.Count();
            var todaySurgeries = dbContext.Surgery.Where(s => s.SurgeryDate.Date == DateTime.Today).Count();
            var totalPatients = dbContext.PatientProfiles.Count();
            var totalTheatres = dbContext.threatres.Count();

            ViewBag.TotalSurgeries = totalSurgeries;
            ViewBag.TodaySurgeries = todaySurgeries;
            ViewBag.TotalPatients = totalPatients;
            ViewBag.TotalTheatres = totalTheatres;

            var urgentPrescriptions = dbContext.NewPrescriptions
      .Include(p => p.PatientProfile)
      .Include(p => p.PrescriptionMedications)
          .ThenInclude(pm => pm.PharmacyMedication)
      .Where(p => p.IsAgent == true && p.Status != "dispensed" && p.Status != "rejected")
      .OrderByDescending(p => p.DateAdded)
      .ToList();

            ViewBag.UrgentPrescriptions = urgentPrescriptions;
            ViewBag.UrgentPrescriptionCount = urgentPrescriptions.Count;

            return View();
        }

        // GET: Prescription/Index
        public ActionResult Index()
        {
            var prescriptions = dbContext.Prescriptions
                .Include("PatientProfile")
                .Include("PharmacyMedication")
                .ToList();
            return View(prescriptions);
        }

        // GET: Prescription/Create
        public ActionResult Create()
        {
            // Populate dropdown lists for PatientProfile and PharmacyMedication
            ViewBag.PatientProfileId = new SelectList(dbContext.PatientProfiles, "PatientProfileId", "PatientName");
            ViewBag.PharmacyMedicationId = new SelectList(dbContext.PharmacyMedication, "PharmacyMedicationId", "Name");
            return View();
        }

        // POST: Prescription/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Prescription prescription)
        {
            
                dbContext.Prescriptions.Add(prescription);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            

            // Repopulate dropdown lists if model validation fails
           // ViewBag.PatientProfileId = new SelectList(dbContext.PatientProfiles, "PatientProfileId", "PatientName", prescription.PatientProfileId);
            //ViewBag.PharmacyMedicationId = new SelectList(dbContext.PharmacyMedication, "PharmacyMedicationId", "Name", prescription.PharmacyMedicationId);
            //return View(prescription);
        }

       

        
        [HttpGet]
        public IActionResult AddPrescription()
        {
            // Populate patients
            ViewBag.Patients = new SelectList(
                dbContext.PatientProfiles,
                "PatientProfileId",
                "PatientIDno"
            );

            // Populate medications sorted by name
            ViewBag.Medications = new SelectList(
                dbContext.PharmacyMedication
                    .OrderBy(m => m.Name), // Sort medications alphabetically by name
                "PharmacyMedicationId",
                "Name"
            );

            return View(new NewPrescriptionViewModel());
        }
        //[HttpPost]
        //public IActionResult AddPrescription(NewPrescriptionViewModel model)
        //{
        //    // Check if the patient has allergies to the active ingredients in the selected medications
        //    var allergicIngredients = dbContext.patientAllergies
        //        .Where(pa => pa.PatientProfileId == model.PatientProfileId)
        //        .Select(pa => pa.ActiveIngredientsID)
        //        .ToList();

        //    var prescribedMedications = model.Medications
        //        .Select(med => med.PharmacyMedicationId)
        //        .ToList();

        //    // Get the active ingredients for the prescribed medications
        //    var prescribedIngredients = dbContext.PharmacyMedication
        //        .Where(pm => prescribedMedications.Contains(pm.PharmacyMedicationId))
        //        .SelectMany(pm => pm.ActiveIngredients.Select(ai => ai.ActiveIngredientsID))
        //        .ToList();

        //    // Check for overlap
        //    var conflictingIngredients = prescribedIngredients.Intersect(allergicIngredients).ToList();

        //    if (conflictingIngredients.Any())
        //    {
        //        // Retrieve ingredient names for the alert
        //        var conflictingIngredientNames = dbContext.ActiveIngredients
        //            .Where(ai => conflictingIngredients.Contains(ai.ActiveIngredientsID))
        //            .Select(ai => ai.ActiveIngredientsName)
        //            .ToList();

        //        TempData["AllergyAlert"] = $"The patient is allergic to the following ingredients: {string.Join(", ", conflictingIngredientNames)}";

        //        // Reload dropdown data for retry
        //        ViewBag.Patients = new SelectList(dbContext.PatientProfiles, "PatientProfileId", "PatientIDno");
        //        ViewBag.Medications = new SelectList(dbContext.PharmacyMedication, "PharmacyMedicationId", "Name");

        //        return View(model);
        //    }

        //    // No allergy conflict, proceed to save the prescription
        //    var prescription = new NewPrescription
        //    {
        //        PatientProfileId = model.PatientProfileId,
        //        Status = model.Status,
        //        IsAgent = model.IsAgent,
        //        //DateAdded = DateTime.Now,
        //        DateAdded = model.DataAdded, // Use the manually added date
        //        Instruction = model.Instruction,
        //        Username = User.Identity.Name
        //    };

        //    dbContext.NewPrescriptions.Add(prescription);
        //    dbContext.SaveChanges();

        //    foreach (var medication in model.Medications)
        //    {
        //        var prescriptionMedication = new PrescriptionMedication
        //        {
        //            PrescriptionId = prescription.PrescriptionId,
        //            PharmacyMedicationId = medication.PharmacyMedicationId,
        //            Quantity = medication.Quantity,
        //            Instruction = medication.Instruction
        //        };

        //        dbContext.PrescriptionMedications.Add(prescriptionMedication);
        //    }

        //    dbContext.SaveChanges();

        //    return RedirectToAction("PrescriptionListSurgeon");
        //}
        [HttpPost]
        public IActionResult AddPrescription(NewPrescriptionViewModel model)
        {
            // Check if the patient has allergies to the active ingredients in the selected medications
            var allergicIngredients = dbContext.patientAllergies
                .Where(pa => pa.PatientProfileId == model.PatientProfileId)
                .Select(pa => pa.ActiveIngredientsID)
                .ToList();

            var prescribedMedications = model.Medications
                .Select(med => med.PharmacyMedicationId)
                .ToList();

            // Get the active ingredients for the prescribed medications
            var prescribedIngredients = dbContext.PharmacyMedication
                .Where(pm => prescribedMedications.Contains(pm.PharmacyMedicationId))
                .SelectMany(pm => pm.ActiveIngredients.Select(ai => ai.ActiveIngredientsID))
                .ToList();

            // Check for overlap
            var conflictingIngredients = prescribedIngredients.Intersect(allergicIngredients).ToList();

            if (conflictingIngredients.Any())
            {
                // Retrieve ingredient names for the alert
                var conflictingIngredientNames = dbContext.ActiveIngredients
                    .Where(ai => conflictingIngredients.Contains(ai.ActiveIngredientsID))
                    .Select(ai => ai.ActiveIngredientsName)
                    .ToList();

                TempData["AllergyAlert"] = $"The patient is allergic to the following ingredients: {string.Join(", ", conflictingIngredientNames)}";
                TempData["ConfirmPrescription"] = Newtonsoft.Json.JsonConvert.SerializeObject(model);

                // Reload dropdown data for retry
                ViewBag.Patients = new SelectList(dbContext.PatientProfiles, "PatientProfileId", "PatientIDno");
                 ViewBag.Medications = new SelectList(dbContext.PharmacyMedication, "PharmacyMedicationId", "Name");

                return View("ConfirmPrescription", model); // Redirect to confirmation page
            }

            // No allergy conflict, proceed to save the prescription
            SavePrescription(model);

            return RedirectToAction("PrescriptionListSurgeon");
        }

        [HttpPost]
        public IActionResult ConfirmPrescription(bool confirm)
        {
            if (confirm)
            {
                var serializedModel = TempData["ConfirmPrescription"] as string;
                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<NewPrescriptionViewModel>(serializedModel);

                SavePrescription(model);

                return RedirectToAction("PrescriptionListSurgeon");
            }

            return RedirectToAction("AddPrescription");
        }

        private void SavePrescription(NewPrescriptionViewModel model)
        {
            var prescription = new NewPrescription
            {
                PatientProfileId = model.PatientProfileId,
                Status = model.Status,
                IsAgent = model.IsAgent,
                DateAdded = model.DataAdded, // Use the manually added date
                Instruction = model.Instruction,
                Username = User.Identity.Name
            };

            dbContext.NewPrescriptions.Add(prescription);
            dbContext.SaveChanges();

            foreach (var medication in model.Medications)
            {
                var prescriptionMedication = new PrescriptionMedication
                {
                    PrescriptionId = prescription.PrescriptionId,
                    PharmacyMedicationId = medication.PharmacyMedicationId,
                    Quantity = medication.Quantity,
                    Instruction = medication.Instruction
                };

                dbContext.PrescriptionMedications.Add(prescriptionMedication);
            }

            dbContext.SaveChanges();
        }



        //[HttpPost]
        //public IActionResult AddPrescription(NewPrescriptionViewModel model)
        //{
        //    //if (!ModelState.IsValid)
        //    //{
        //    //    // Reload dropdown data in case of validation errors
        //    //    ViewBag.Patients = new SelectList(dbContext.PatientProfiles, "PatientProfileId", "PatientIDno");
        //    //    ViewBag.Medications = new SelectList(dbContext.PharmacyMedication, "PharmacyMedicationId", "Name");
        //    //    return View(model);
        //    //}

        //    // Create a new prescription record
        //    var prescription = new NewPrescription
        //    {

        //        PatientProfileId = model.PatientProfileId,

        //        Status = model.Status,
        //        IsAgent = model.IsAgent,
        //        DateAdded = DateTime.Now,
        //        Instruction = model.Instruction,
        //        Username = User.Identity.Name // Fetch the logged-in user's username
        //    };

        //    dbContext.NewPrescriptions.Add(prescription);
        //    dbContext.SaveChanges();

        //    // Loop through the medications and add them to the PrescriptionMedication table
        //    foreach (var medication in model.Medications)
        //    {
        //        var prescriptionMedication = new PrescriptionMedication
        //        {
        //            PrescriptionId = prescription.PrescriptionId,
        //            PharmacyMedicationId = medication.PharmacyMedicationId,
        //            Quantity = medication.Quantity,
        //            Instruction = medication.Instruction
        //        };

        //        dbContext.PrescriptionMedications.Add(prescriptionMedication);
        //    }

        //    dbContext.SaveChanges();

        //    // Redirect to the list or confirmation page after saving
        //    return RedirectToAction("PrescriptionListSurgeon"); // Adjust to your actual redirect action
        //}
        //public IActionResult PrescriptionListSurgeon()
        //{
        //    // Fetch prescriptions and include related data
        //    var prescriptions = dbContext.NewPrescriptions
        //        .Include(p => p.PatientProfile) // Include related PatientProfile data
        //        .Include(p => p.PrescriptionMedications)
        //            .ThenInclude(pm => pm.PharmacyMedication) // Include related medications
        //        .ToList();

        //    return View(prescriptions);
        //}
        public IActionResult PrescriptionListSurgeon(DateTime? startDate, DateTime? endDate)
        {
            // Fetch all prescriptions initially
            var prescriptions = dbContext.NewPrescriptions
                .Include(p => p.PatientProfile) // Include related PatientProfile data
                .Include(p => p.PrescriptionMedications)
                    .ThenInclude(pm => pm.PharmacyMedication) // Include related medications
                .AsQueryable();

            // Apply date filtering if provided
            if (startDate.HasValue && endDate.HasValue)
            {
                prescriptions = prescriptions
                    .Where(p => p.DateAdded.Date >= startDate.Value.Date && p.DateAdded.Date <= endDate.Value.Date);
            }

            return View(prescriptions.ToList());
        }





        //public IActionResult PrescriptionList(string statusFilter, string agentFilter, string patientIdSearch)
        //{
        //    // Fetch prescriptions from the database
        //    var prescriptions = dbContext.NewPrescriptions
        //        .Include(p => p.PatientProfile)
        //        .Include(p => p.PharmacyMedication)
        //        .AsQueryable();

        //    // Apply filters
        //    if (!string.IsNullOrEmpty(statusFilter))
        //    {
        //        prescriptions = prescriptions.Where(p => p.Status == statusFilter);
        //    }

        //    if (!string.IsNullOrEmpty(agentFilter))
        //    {
        //        if (agentFilter == "Agent")
        //        {
        //            prescriptions = prescriptions.Where(p => p.IsAgent);
        //        }
        //        else if (agentFilter == "Non-Agent")
        //        {
        //            prescriptions = prescriptions.Where(p => !p.IsAgent);
        //        }
        //    }

        //    // Apply patient ID search
        //    if (!string.IsNullOrEmpty(patientIdSearch))
        //    {
        //        prescriptions = prescriptions.Where(p => p.PatientProfile.PatientIDno.Contains(patientIdSearch));
        //        ViewBag.PatientIDSearch = patientIdSearch;
        //    }

        //    // Pass the filtered results to the view
        //    return View(prescriptions.ToList());
        //}

        public IActionResult PrescriptionList(string statusFilter, string agentFilter, string patientIdSearch)
        {
            // Fetch prescriptions from the database
            var prescriptions = dbContext.NewPrescriptions
                .Include(p => p.PatientProfile) // Include related PatientProfile
                .Include(p => p.PrescriptionMedications) // Include related PrescriptionMedications
                .ThenInclude(pm => pm.PharmacyMedication) // Include related PharmacyMedication
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(statusFilter))
            {
                prescriptions = prescriptions.Where(p => p.Status == statusFilter);
            }

            if (!string.IsNullOrEmpty(agentFilter))
            {
                if (agentFilter == "Agent")
                {
                    prescriptions = prescriptions.Where(p => p.IsAgent);
                }
                else if (agentFilter == "Non-Agent")
                {
                    prescriptions = prescriptions.Where(p => !p.IsAgent);
                }
            }

            if (!string.IsNullOrEmpty(patientIdSearch))
            {
                prescriptions = prescriptions.Where(p => p.PatientProfile.PatientIDno.Contains(patientIdSearch));
                ViewBag.PatientIDSearch = patientIdSearch;
            }

            // Pass the filtered results to the view
            return View(prescriptions.ToList());
        }






        //[HttpPost]
        //public IActionResult DispenseMedication(int prescriptionId, int quantity)
        //{
        //    // Retrieve the prescription using the prescriptionId
        //    var prescription = dbContext.NewPrescriptions
        //        .Include(p => p.PharmacyMedication)
        //        .FirstOrDefault(p => p.PrescriptionId == prescriptionId);

        //    if (prescription == null)
        //    {
        //        // Handle the case where the prescription is not found
        //        return NotFound();
        //    }

        //    // Check if there's enough stock to dispense
        //    if (prescription.PharmacyMedication.StockOnHand < quantity)
        //    {
        //        // Handle insufficient stock scenario
        //        ModelState.AddModelError("", "Insufficient stock to dispense.");
        //        return RedirectToAction("PrescriptionList");
        //    }

        //    // Update the stock on hand in PharmacyMedication
        //    prescription.PharmacyMedication.StockOnHand -= quantity;

        //    // Update the status of the prescription to "Dispensed"
        //    prescription.Status = "Dispensed";

        //    // Save changes to the database
        //    dbContext.SaveChanges();

        //    // Redirect back to the prescription list
        //    return RedirectToAction("PrescriptionList");
        //}


        // Action to display the rejection view
        public IActionResult RejectPrescription(int prescriptionId)
        {
            var prescription = dbContext.NewPrescriptions
                .Include(p => p.PatientProfile)
                .Include(p => p.PharmacyMedication)
                .FirstOrDefault(p => p.PrescriptionId == prescriptionId);

            if (prescription == null)
            {
                return NotFound();
            }

            return View(prescription);
        }

        // Action to process the rejection
        [HttpPost]
        public IActionResult Reject(int prescriptionId, string rejectionNote)
        {
            var prescription = dbContext.NewPrescriptions.FirstOrDefault(p => p.PrescriptionId == prescriptionId);

            if (prescription == null)
            {
                return NotFound();
            }

            // Update the status and rejection note
            prescription.Status = "Rejected";
            prescription.RejectionNote = rejectionNote;

            // Capture the username of the person rejecting the prescription
            prescription.Username = User.Identity.Name;

            // Save changes to the database
            dbContext.SaveChanges();

            // Redirect to the prescription list or another relevant page
            return RedirectToAction("PrescriptionList"); // Adjust according to your routing
        }

     

        public IActionResult ManagePrescription(int prescriptionId)
        {
            var prescription = dbContext.NewPrescriptions
                .Include(p => p.PatientProfile)
                .Include(p => p.PrescriptionMedications)
                    .ThenInclude(pm => pm.PharmacyMedication)
                        .ThenInclude(pm => pm.DosageForm)
                .FirstOrDefault(p => p.PrescriptionId == prescriptionId);

            if (prescription == null)
            {
                return NotFound();
            }

            var viewModel = new ManagePrescriptionViewModel
            {
                PrescriptionId = prescription.PrescriptionId,
                PatientName = prescription.PatientProfile?.PatientIDno ?? "Unknown",
                Status = prescription.Status,
                DateAdded = prescription.DateAdded,
                IsAgent = prescription.IsAgent,
                Instruction = prescription.Instruction,
                RejectionNote = prescription.RejectionNote,
                Medications = prescription.PrescriptionMedications.Select(pm => new MedicationDetail
                {
                    MedicationName = pm.PharmacyMedication?.Name ?? "Unknown",
                    Quantity = pm.Quantity,
                    DosageForm = pm.PharmacyMedication?.DosageForm?.DosageFormName ?? "Unknown",
                    Instruction = pm.Instruction ?? ""
                }).ToList()
            };

            return View(viewModel);
        }






        // GET: PatientAllergy/Add
        public ActionResult Add()
        {
            // Prepare ViewBag for dropdowns
            ViewBag.PatientProfileId = new SelectList(dbContext.PatientProfiles, "PatientProfileId", "PatientName");
            ViewBag.ActiveIngredientsID = new SelectList(dbContext.ActiveIngredients, "ActiveIngredientsID", "ActiveIngredientsName");
            return View();
        }

        // POST: PatientAllergy/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(PatientAllergy model)
        {
          
                // Create a new PatientAllergy based on the ViewModel
                var patientAllergy = new PatientAllergy
                {
                    PatientProfileId = model.PatientProfileId,
                    ActiveIngredientsID = model.ActiveIngredientsID,
                    AllergyDescription = model.AllergyDescription
                };

                // Add to the database
                dbContext.patientAllergies.Add(patientAllergy);
                dbContext.SaveChanges();
                return RedirectToAction("Add"); // Redirect to a relevant page after adding
            

            // If model state is invalid, repopulate dropdowns
            //ViewBag.PatientProfileId = new SelectList(dbContext.PatientProfiles, "PatientProfileId", "PatientName", model.PatientProfileId);
            //ViewBag.ActiveIngredientsID = new SelectList(dbContext.ActiveIngredients, "ActiveIngredientsID", "ActiveIngredientsName", model.ActiveIngredientsID);
            //return View(model);
        }

     

        public ActionResult AddMultiplePatientMedications()
        {
            // Prepare ViewBag for dropdowns
            ViewBag.PatientProfileId = new SelectList(dbContext.PatientProfiles, "PatientProfileId", "PatientName");
            ViewBag.PharmacyMedicationId = new SelectList(dbContext.PharmacyMedication, "PharmacyMedicationId", "Name");

            var model = new List<PatientMedication>(); // Pass a list to the view
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMultiplePatientMedications(List<PatientMedication> model)
        {
           
                foreach (var patientMedication in model)
                {
                    // Add each medication entry to the database
                    dbContext.PatientMedications.Add(patientMedication);
                }
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            

            // If model state is invalid, repopulate dropdowns for each item
            //ViewBag.PatientProfileId = new SelectList(dbContext.PatientProfiles, "PatientProfileId", "PatientName");
            //ViewBag.PharmacyMedicationId = new SelectList(dbContext.PharmacyMedication, "PharmacyMedicationId", "Name");

            //return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddMultiplePatientMedications(List<PatientMedication> model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        foreach (var patientMedication in model)
        //        {
        //            // Ensure PatientProfileId and PharmacyMedicationId are set for each entry
        //            if (patientMedication.PatientProfileId > 0 && patientMedication.PharmacyMedicationId > 0)
        //            {
        //                dbContext.PatientMedications.Add(patientMedication);
        //            }
        //        }

        //        dbContext.SaveChanges();
        //        return RedirectToAction("Index"); // Redirect to a relevant page after adding
        //    }

        //    // If model state is invalid, repopulate dropdowns
        //    ViewBag.PatientProfileId = new SelectList(dbContext.PatientProfiles, "PatientProfileId", "PatientName");
        //    ViewBag.PharmacyMedicationId = new SelectList(dbContext.PharmacyMedication, "PharmacyMedicationId", "Name");

        //    return View(model);
        //}


        // GET: Vitals/Add
        public ActionResult AddVital()
        {
            var viewModel = new VitalSignPageViewModel
            {
                NewVitalSign = new VitalSign(),
                ExistingVitalSigns = dbContext.VitalSigns
              .Include(v => v.PatientProfile)
              .ToList()
            };

            // Create full name by combining first and surname
            var patientList = dbContext.PatientProfiles
                .Select(p => new
                {
                    p.PatientProfileId,
                    FullName = p.PatientName + " " + p.PSurname
                }).ToList();

            ViewBag.PatientList = new SelectList(patientList, "PatientProfileId", "FullName");

            return View(viewModel);

        }

        // POST: Vitals/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVital(VitalSignPageViewModel model)
        {

            //    model.NewVitalSign.Username = User.Identity.Name;
            //    model.NewVitalSign.RecordedAt = DateTime.Now;

            //    dbContext.VitalSigns.Add(model.NewVitalSign);
            //    dbContext.SaveChanges();




            //// Repopulate dropdown and data on error
            //model.ExistingVitalSigns = dbContext.VitalSigns.ToList();
            //ViewBag.PatientIDno = new SelectList(dbContext.PatientProfiles, "PatientPatientId", "PatientIDno", model.NewVitalSign.PatientProfileId);
          
                model.NewVitalSign.Username = User.Identity.Name;
                model.NewVitalSign.RecordedAt = DateTime.Now;

                dbContext.VitalSigns.Add(model.NewVitalSign);
                dbContext.SaveChanges();

             
            

            // Repopulate dropdown and existing vitals on error
            var patientList = dbContext.PatientProfiles
                .Select(p => new
                {
                    p.PatientProfileId,
                    FullName = p.PatientName + " " + p.PSurname
                }).ToList();
            ViewBag.PatientList = new SelectList(patientList, "PatientProfileId", "FullName");

            model.ExistingVitalSigns = dbContext.VitalSigns
                .Include(v => v.PatientProfile)
                .ToList();

            return View(model);

            //return View(model);


        }
        // GET: Vitals/Index
        public ActionResult ListVitals()
        {
            var vitals = dbContext.VitalSigns.Include("PatientProfile").ToList();
            return View(vitals);
        }

        // GET: View patient condition
        public IActionResult ViewPatientCondition(int patientId)
        {
            var patientConditions = dbContext.PatientConditions.OrderBy(p => p.Condition.Diagnosis)
                .Include(pc => pc.Condition)
                .Where(pc => pc.PatientProfileId == patientId)
                .ToList();

            if (patientConditions == null || patientConditions.Count == 0)
            {
                return NotFound();
            }

            return View(patientConditions); // Pass conditions to the view
        }

      

        // GET: View patient allergy
        public IActionResult ViewPatientAllergy(int patientId)
        {
            var patientAllergies = dbContext.patientAllergies
                .Include(pa => pa.ActiveIngredients)
                .Where(pa => pa.PatientProfileId == patientId)
                .ToList();

            if (patientAllergies == null || patientAllergies.Count == 0)
            {
                return NotFound();
            }

            return View(patientAllergies); // Pass allergies to the view
        }
        // GET: View patient allergy
        public IActionResult ViewPatientVitals(int patientId)
        {
            var patientVitals = dbContext.VitalSigns
                .Include(pa => pa.PatientProfile)
                .Where(pa => pa.PatientProfileId == patientId)
                .ToList();

            if (patientVitals == null || patientVitals.Count == 0)
            {
                return NotFound();
            }

            return View(patientVitals); // Pass allergies to the view
        }




        //[HttpPost]
        //public IActionResult Dispense(int prescriptionId, int quantity)
        //{
        //    // Retrieve the prescription using the prescriptionId
        //    var prescription = dbContext.NewPrescriptions
        //        .Include(p => p.PharmacyMedication)
        //        .ThenInclude(pm => pm.MedicationActiveIngredients)
        //        .FirstOrDefault(p => p.PrescriptionId == prescriptionId);

        //    if (prescription == null)
        //    {
        //        // Handle the case where the prescription is not found
        //        return NotFound();
        //    }

        //    // Retrieve the patient's allergies
        //    var patientAllergies = dbContext.patientAllergies
        //        .Where(pa => pa.PatientProfileId == prescription.PatientProfileId)
        //        .Select(pa => pa.ActiveIngredientsID)
        //        .ToList();

        //    // Check if the patient is allergic to any of the active ingredients in the medication
        //    var medicationIngredients = prescription.PharmacyMedication.MedicationIngredients
        //        .Select(ma => ma.ActiveIngredientsID)
        //        .ToList();

        //    bool hasAllergy = medicationIngredients.Any(mi => patientAllergies.Contains(mi));

        //    if (hasAllergy)
        //    {
        //        // Display a warning to the user if the patient is allergic to the medication
        //        TempData["AllergyWarning"] = "This patient is allergic to one or more active ingredients in this medication.";
        //        TempData["PrescriptionId"] = prescriptionId;
        //        TempData["Quantity"] = quantity;
        //        return RedirectToAction("ConfirmDispense"); // Redirect to a confirmation page
        //    }

        //    // Proceed with dispensing the medication if there's no allergy
        //    if (prescription.PharmacyMedication.StockOnHand < quantity)
        //    {
        //        // Handle insufficient stock scenario
        //        ModelState.AddModelError("", "Insufficient stock to dispense.");
        //        return RedirectToAction("PrescriptionList");
        //    }

        //    // Update the stock on hand in PharmacyMedication
        //    prescription.PharmacyMedication.StockOnHand -= quantity;

        //    // Update the status of the prescription to "Dispensed"
        //    prescription.Status = "Dispensed";

        //    // Save changes to the database
        //    dbContext.SaveChanges();

        //    // Redirect back to the prescription list
        //    return RedirectToAction("PrescriptionList");
        //}
        [HttpPost]
        public IActionResult Dispense(int prescriptionId, int medicationId, int quantity)
        {
            // Get the prescription
            var prescription = dbContext.NewPrescriptions
                .Include(p => p.PrescriptionMedications)
                .FirstOrDefault(p => p.PrescriptionId == prescriptionId);

            if (prescription == null)
            {
                return NotFound("Prescription not found.");
            }

            // Get the specific medication for this prescription
            var prescriptionMedication = dbContext.PrescriptionMedications
                .Include(pm => pm.PharmacyMedication)
                    .ThenInclude(m => m.MedicationActiveIngredients)
                .FirstOrDefault(pm => pm.PrescriptionId == prescriptionId && pm.PharmacyMedicationId == medicationId);

            if (prescriptionMedication == null)
            {
                return NotFound("Medication in prescription not found.");
            }

            var medication = prescriptionMedication.PharmacyMedication;

            // Get patient's allergy ingredient IDs
            var allergyIngredientIds = dbContext.patientAllergies
                .Where(pa => pa.PatientProfileId == prescription.PatientProfileId)
                .Select(pa => pa.ActiveIngredientsID)
                .ToList();

            // Get medication ingredient IDs
            var medicationIngredientIds = medication.MedicationActiveIngredients
                .Select(mai => mai.ActiveIngredientsID)
                .ToList();

            // Check for allergy conflict
            bool hasAllergy = medicationIngredientIds.Any(id => allergyIngredientIds.Contains(id));
            if (hasAllergy)
            {
                TempData["AllergyWarning"] = "This patient is allergic to one or more ingredients in this medication.";
                TempData["PrescriptionId"] = prescriptionId;
                TempData["MedicationId"] = medicationId;
                TempData["Quantity"] = quantity;
                return RedirectToAction("ConfirmDispense");
            }

            // Check stock
            if (medication.StockOnHand < quantity)
            {
                ModelState.AddModelError("", "Not enough stock to dispense.");
                return RedirectToAction("PrescriptionList");
            }

            // Dispense medication
            medication.StockOnHand -= quantity;

            // Optional: update status of the specific PrescriptionMedication
            prescriptionMedication.Instruction += " (Dispensed on " + DateTime.Now.ToShortDateString() + ")";

            // You could also update the prescription status globally if all items are dispensed
            prescription.Status = "Dispensed";

            dbContext.SaveChanges();

            TempData["Success"] = "Medication dispensed successfully.";
            return RedirectToAction("PrescriptionList");
        }


        //public IActionResult ConfirmDispense()
        //{
        //    // Retrieve the warning message from TempData
        //    ViewBag.AllergyWarning = TempData["AllergyWarning"];
        //    ViewBag.PrescriptionId = TempData["PrescriptionId"];
        //    ViewBag.Quantity = TempData["Quantity"];

        //    return View();
        //}

        //[HttpPost]
        //public IActionResult ConfirmDispense(int prescriptionId, int quantity, bool proceed)
        //{
        //    if (proceed)
        //    {
        //        // Proceed with dispensing the medication
        //        return Dispense(prescriptionId, quantity); // Call the Dispense method
        //    }

        //    // Redirect back to the Manage Prescription page if user cancels
        //    return RedirectToAction("ManagePrescription", new { id = prescriptionId });
        //}

        //public IActionResult CheckContraIndication(int prescriptionId)
        //{
        //    var prescription = dbContext.NewPrescriptions
        //        .Include(p => p.PatientProfile)
        //        .Include(p => p.PharmacyMedication)
        //        .ThenInclude(m => m.ContraIndications)
        //        .FirstOrDefault(p => p.PrescriptionId == prescriptionId);

        //    if (prescription == null)
        //    {
        //        return NotFound();
        //    }

        //    var patientConditions = dbContext.PatientConditions
        //        .Where(pc => pc.PatientProfileId == prescription.PatientProfileId)
        //        .Include(pc => pc.Condition)
        //        .ToList();

        //    var contraindications = new List<ContraIndication>();

        //    foreach (var condition in patientConditions)
        //    {
        //        var contraIndicationsForCondition = prescription.PharmacyMedication
        //            .ContraIndications
        //            .Where(ci => ci.ConditionID == condition.ConditionID)
        //            .ToList();

        //        contraindications.AddRange(contraIndicationsForCondition);
        //    }

        //    if (contraindications.Any())
        //    {
        //        // Show alert to pharmacist
        //        TempData["ContraIndicationAlert"] = "Contraindication found with the patient's condition.";
        //        return RedirectToAction("ManagePrescription", new { id = prescriptionId });
        //    }

        //    return RedirectToAction("Dispense", new { id = prescriptionId });
        //}


        //[HttpPost]
        //public IActionResult CheckContraIndication(int prescriptionId)
        //{
        //    var prescription = dbContext.NewPrescriptions
        //        .Include(p => p.PatientProfile)
        //        .Include(p => p.PharmacyMedication)
        //         .ThenInclude(pm => pm.MedicationActiveIngredients) // Assuming MedicationActiveIngredients is a navigation property for active ingredients in the medication
        //        //.ThenInclude(m => m.ContraIndications)
        //        .FirstOrDefault(p => p.PrescriptionId == prescriptionId);

        //    if (prescription == null)
        //    {
        //        return NotFound();
        //    }

        //    var patientConditions = dbContext.PatientConditions
        //        .Where(pc => pc.PatientProfileId == prescription.PatientProfileId)
        //        .Include(pc => pc.Condition)
        //        .ToList();

        //    var contraindications = new List<ContraIndication>();

        //    foreach (var condition in patientConditions)
        //    {
        //        var contraIndicationsForCondition = prescription.PharmacyMedication
        //            .ContraIndications
        //            .Where(ci => ci.ConditionID == condition.ConditionID)
        //            .ToList();

        //        contraindications.AddRange(contraIndicationsForCondition);
        //    }

        //    if (contraindications.Any())
        //    {
        //        // Show an alert that contraindications exist
        //        TempData["ContraIndicationAlert"] = "Alert Contraindication(s) found with the patient's condition. Click prescription list on the side bar go back to the prescription list, dispense or reject";
        //    }
        //    else
        //    {
        //        TempData["ContraIndicationAlert"] = "No contraindications found.";
        //    }

        //    // Return the same view with updated prescription details
        //    return View("ManagePrescription", prescription);
        //}


        //[HttpPost]
        //public IActionResult CheckContraIndication(int prescriptionId)
        //{
        //    // Get the prescription along with the patient profile and active ingredients of the prescribed medication
        //    var prescription = dbContext.NewPrescriptions
        //        .Include(p => p.PatientProfile)
        //        .Include(p => p.PharmacyMedication)
        //        .ThenInclude(pm => pm.MedicationActiveIngredients) // Assuming MedicationActiveIngredients is a navigation property for active ingredients in the medication
        //        .FirstOrDefault(p => p.PrescriptionId == prescriptionId);

        //    if (prescription == null)
        //    {
        //        return NotFound();
        //    }

        //    // Get the patient's conditions
        //    var patientConditions = dbContext.PatientConditions
        //        .Where(pc => pc.PatientProfileId == prescription.PatientProfileId)
        //        .Include(pc => pc.Condition)
        //        .ToList();

        //    var contraindications = new List<ContraIndication>();

        //    // Iterate over each condition the patient has
        //    foreach (var condition in patientConditions)
        //    {
        //        // Check each active ingredient of the prescribed medication
        //        foreach (var activeIngredient in prescription.PharmacyMedication.MedicationActiveIngredients)
        //        {
        //            // Find any contraindications where the active ingredient and condition match
        //            var contraIndicationsForCondition = dbContext.ContraIndications
        //                .Where(ci => ci.ActiveIngredientsID == activeIngredient.ActiveIngredientsID && ci.ConditionID == condition.ConditionID)
        //                .ToList();

        //            // Add found contraindications to the list
        //            contraindications.AddRange(contraIndicationsForCondition);
        //        }
        //    }

        //    // Handle results: if any contraindications were found, alert the user
        //    if (contraindications.Any())
        //    {
        //        TempData["ContraIndicationAlert"] = "Alert: Contraindication(s) found with the patient's condition. Click prescription list on the sidebar to go back, dispense or reject.";
        //    }
        //    else
        //    {
        //        TempData["ContraIndicationAlert"] = "No contraindications found.";
        //    }

        //    // Return the same view with updated prescription details
        //    return View("ManagePrescription", prescription);
        //}



        //public IActionResult CheckContraIndication(int prescriptionId)
        //{
        //    var prescription = dbContext.NewPrescriptions
        //        .Include(p => p.PatientProfile)
        //        .Include(p => p.PharmacyMedication)
        //        .ThenInclude(m => m.ContraIndications)
        //        .FirstOrDefault(p => p.PrescriptionId == prescriptionId);

        //    if (prescription == null)
        //    {
        //        return NotFound();
        //    }

        //    var patientConditions = dbContext.PatientConditions
        //        .Where(pc => pc.PatientProfileId == prescription.PatientProfileId)
        //        .Include(pc => pc.Condition)
        //        .ToList();

        //    var contraindications = new List<ContraIndication>();

        //    foreach (var condition in patientConditions)
        //    {
        //        var contraIndicationsForCondition = prescription.PharmacyMedication
        //            .ContraIndications
        //            .Where(ci => ci.ConditionID == condition.ConditionID)
        //            .ToList();

        //        contraindications.AddRange(contraIndicationsForCondition);
        //    }

        //    if (contraindications.Any())
        //    {
        //        // Show an alert to the pharmacist that there are contraindications
        //        TempData["ContraIndicationAlert"] = "Contraindication(s) found with the patient's condition.";
        //        return RedirectToAction("ManagePrescription", new { id = prescriptionId });
        //    }

        //    return RedirectToAction("Dispense", new { id = prescriptionId });
        //}




















        [HttpGet]

        public ActionResult CreateSurgery()
        {
            ViewBag.PatientProfiles = new SelectList(dbContext.PatientProfiles, "PatientProfileId", "PatientIDno");
            ViewBag.Theatres = new SelectList(dbContext.threatres, "TheatreID", "TheatreName");
            ViewBag.TreatmentCodes = new SelectList(dbContext.TreatmentCodes, "TreatmentCodeId", "Description");

            return View();

        }



        


        [HttpPost]
        public ActionResult CreateSurgery(Surgery surgery)
        {
          
                surgery.CreatedBy = User.Identity.Name;  // Logged in user
                surgery.CreatedDate = DateTime.Now;

                dbContext.Surgery.Add(surgery);
                dbContext.SaveChanges();

                return RedirectToAction("ListofSurgery");
            

           // return View(surgery);
        }

        public ActionResult ListofSurgery()
        {
            var surgeries = dbContext.Surgery
                .Include(s => s.PatientProfile)
                .Include(s => s.Theatre)
                //.Include(s => s.TreatmentCode)
                .ToList();

            return View(surgeries);
        }

        // View to display the list of prescriptions
        //public IActionResult PrescriptionListSurgeon()
        //{
        //    var prescriptions = dbContext.NewPrescriptions
        //        .Include(p => p.PatientProfile) // Assuming you want to include related patient details
        //        .Include(p => p.PharmacyMedication) // Assuming you want to include related medication details
        //        .ToList();

        //    return View(prescriptions);
        //}

        //// GET: Search Prescription by ID or Patient ID
        //public ActionResult SearchPrescription(string searchText)
        //{
        //    // Check if the search text is provided
        //    if (string.IsNullOrEmpty(searchText))
        //    {
        //        return View(new List<NewPrescription>()); // Return an empty list if no search text
        //    }

        //    // Query the prescriptions based on search text
        //    var results = dbContext.NewPrescriptions
        //        .Include(p => p.PatientProfile)
        //        .Include(p => p.PharmacyMedication)
        //        .Where(p => p.PrescriptionId.ToString().Contains(searchText) ||
        //                    p.PatientProfile.PatientIDno.Contains(searchText))
        //        .ToList();

        //    // Return the search results to the view
        //    return View(results);
        //}

















    }
}

