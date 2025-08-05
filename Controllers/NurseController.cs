using E_PRESCRIBING_SYSTEM.Data;
using E_PRESCRIBING_SYSTEM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_PRESCRIBING_SYSTEM.Controllers
{
    public class NurseController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public NurseController(ApplicationDbContext dBD)
        {
            dbContext = dBD;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult NurseDashboard()
        {
            var totalPatients = dbContext.PatientProfiles.Count();
            var totalAdmissions = dbContext.Admissions.Count();
            var recentAdmissions = dbContext.Admissions
                .Include(a => a.PatientProfile)
                .Include(a => a.Ward)
                .OrderByDescending(a => a.Date)
                .Take(5)
                .ToList();

            var recentVitals = dbContext.VitalSigns
                .Include(v => v.PatientProfile)
                .OrderByDescending(v => v.RecordedAt)
                .Take(5)
                .ToList();

            var recentAllergies = dbContext.patientAllergies
                .Include(a => a.PatientProfile)
                .Include(a => a.ActiveIngredients)
                .OrderByDescending(a => a.PatientAllergyId)
                .Take(5)
                .ToList();

            var recentConditions = dbContext.PatientConditions
                .Include(pc => pc.PatientProfile)
                .Include(pc => pc.Condition)
                .OrderByDescending(pc => pc.PatientConditionId)
                .Take(5)
                .ToList();

            ViewBag.TotalPatients = totalPatients;
            ViewBag.TotalAdmissions = totalAdmissions;
            ViewBag.RecentAdmissions = recentAdmissions;
            ViewBag.RecentVitals = recentVitals;
            ViewBag.RecentAllergies = recentAllergies;
            ViewBag.RecentConditions = recentConditions;

            return View();
           
        }

        // GET: PatientCondition/Add
        public ActionResult PatientCondition()
        {
            // Prepare ViewBag for dropdowns
            ViewBag.PatientProfileId = new SelectList(dbContext.PatientProfiles, "PatientProfileId", "PatientName");
            ViewBag.ConditionID = new SelectList(dbContext.conditions, "ConditionID", "Diagnosis");
            //return View();
            var patientConditions = dbContext.PatientConditions
       .Include(pc => pc.PatientProfile)
       .Include(pc => pc.Condition)
       .ToList()
       .Select(pc => new
       {
           PatientName = pc.PatientProfile.PatientName,
           ConditionName = pc.Condition.Diagnosis,
           DateDiagnosed = pc.DiagnosisDate
       });

            ViewBag.PatientConditions = patientConditions;

            return View("PatientCondition");
        }

        // POST: PatientCondition/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PatientCondition(PatientCondition model, List<int> ConditionID)
        {

            // Iterate over each selected ConditionID and create a new PatientCondition entry
            foreach (var conditionId in ConditionID)
            {
                var newPatientCondition = new PatientCondition  
                {
                    PatientProfileId = model.PatientProfileId,
                    ConditionID = conditionId,
                    DiagnosisDate = model.DiagnosisDate
                };
                dbContext.PatientConditions.Add(newPatientCondition);
            }

            dbContext.SaveChanges();



            // Repopulate dropdowns if the model state is invalid
            ViewBag.PatientProfileId = new SelectList(dbContext.PatientProfiles, "PatientProfileId", "PatientName", model.PatientProfileId);
            ViewBag.ConditionID = new SelectList(dbContext.conditions, "ConditionID", "Diagnosis");
            return RedirectToAction("PatientCondition");
        }
        public ActionResult PatientAllergies()
        {
            //      // Prepare ViewBag for dropdowns
            //      ViewBag.PatientProfileId = new SelectList(dbContext.PatientProfiles, "PatientProfileId", "PatientName");
            //      ViewBag.ActiveIngredientsId = new SelectList(dbContext.ActiveIngredients, "ActiveIngredientsID", "ActiveIngredientsName");

            //      ViewBag.PatientConditions = dbContext.PatientConditions
            //    .Include(pc => pc.PatientProfile)
            //    .Include(pc => pc.Condition)
            //     .Select(pc => new
            //     {
            //         PatientName = pc.PatientProfile.FullName,
            //         ConditionName = pc.Condition.ConditionName,
            //         TreatmentCode = pc.TreatmentCode,
            //         Description = pc.Description,
            //         DateDiagnosed = pc.DateDiagnosed
            //     })
            //.ToList();

            //      return View();
            // Dropdowns
            ViewBag.PatientProfileId = new SelectList(dbContext.PatientProfiles, "PatientProfileId", "PatientName");
            ViewBag.ActiveIngredientsId = new SelectList(dbContext.ActiveIngredients, "ActiveIngredientsID", "ActiveIngredientsName");


            ViewBag.PatienAllergies = dbContext.patientAllergies
       .Include(pa => pa.PatientProfile)
       .Include(pa => pa.ActiveIngredients)
       .Select(pa => new PatientAllergyViewModel
       {
           PatientName = pa.PatientProfile.PatientName,
           ActiveIngredientName = pa.ActiveIngredients.ActiveIngredientsName,
           AllergyDescription = pa.AllergyDescription
       })
       .ToList();

            // Patient Conditions data for display
            ViewBag.PatientConditions = dbContext.PatientConditions
                .Include(pc => pc.PatientProfile)
                .Include(pc => pc.Condition)
                .Select(pc => new PatientConditionViewModel
                {
                    PatientName = pc.PatientProfile.PatientName,
                    ConditionName = pc.Condition.Diagnosis,
                   // TreatmentCode = pc.,
                   // Description = pc.Description,
                    DateDiagnosed = pc.DiagnosisDate
                })
                .ToList();

            return View();
        }

        // POST: PatientAllergy/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PatientAllergies(PatientAllergy model, List<int> ActiveIngredientsID)
        {

            // Iterate over each selected ActiveIngredientsID and create a new PatientAllergy entry
            foreach (var allergyId in ActiveIngredientsID)
            {
                var newPatientAllergy = new PatientAllergy
                {
                    PatientProfileId = model.PatientProfileId,
                    ActiveIngredientsID = allergyId,
                    AllergyDescription = model.AllergyDescription,
                };
                dbContext.patientAllergies.Add(newPatientAllergy);
            }

            dbContext.SaveChanges();


            // Repopulate dropdowns if the model state is invalid
            ViewBag.PatientProfileId = new SelectList(dbContext.PatientProfiles, "PatientProfileId", "PatientName", model.PatientProfileId);
            ViewBag.ActiveIngredientsId = new SelectList(dbContext.ActiveIngredients, "ActiveIngredientsID", "ActiveIngredientsName");

            return RedirectToAction("PatientAllergies");
        }

        //[HttpGet]
        //public ActionResult PatientAllergies()
        //{
        //    ViewBag.PatientProfileId = new SelectList(dbContext.PatientProfiles, "PatientProfileId", "PatientName");
        //    ViewBag.ActiveIngredientsId = dbContext.ActiveIngredients
        //        .Select(ai => new SelectListItem
        //        {
        //            Value = ai.ActiveIngredientsID.ToString(),
        //            Text = ai.ActiveIngredientsName
        //        })
        //        .ToList();

        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult PatientAllergies(PatientAllergy model, List<int> ActiveIngredients)
        //{
        //    foreach (var ingredientId in ActiveIngredients)
        //    {
        //        var newPatientAllergy = new PatientAllergy
        //        {
        //            PatientProfileId = model.PatientProfileId,
        //            ActiveIngredientsID = ingredientId,
        //            AllergyDescription = model.AllergyDescription
        //        };
        //        dbContext.patientAllergies.Add(newPatientAllergy);
        //    }

        //    dbContext.SaveChanges();
        //    return RedirectToAction("PatientAllergies");
        //}
        //// GET: Admissions/Create
        //public ActionResult Create()
        //{
        //    ViewBag.PatientProfiles = new SelectList(dbContext.PatientProfiles, "PatientProfileId", "PatientName");
        //    ViewBag.Wards = new SelectList(dbContext.Wards, "WardID", "WardName");
        //    return View();
        //}



        // GET: Admissions/Create
        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new AdmissionPageViewModel
            {
                Admissions = dbContext.Admissions.Include(a => a.PatientProfile).ToList(),
                NewAdmission = new Admission()
            };

            ViewBag.PatientProfiles = new SelectList(dbContext.PatientProfiles, "PatientProfileId", "PatientName");
            ViewBag.Wards = new SelectList(dbContext.Wards, "WardID", "WardName");

            return View(viewModel);
        }

        // POST: Admissions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Admission admission)
        {
          
                // Add the username of the logged-in user
                admission.Username = User.Identity.Name;
                admission.Date = DateTime.Now;

                dbContext.Admissions.Add(admission);
                dbContext.SaveChanges();

               
           

            // Re-populate dropdowns in case of validation errors
            ViewBag.PatientProfiles = new SelectList(dbContext.PatientProfiles, "PatientProfileId", "PatientName", admission.PatientProfileId);
            ViewBag.Wards = new SelectList(dbContext.Wards, "WardID", "WardName", admission.WardID);

            return RedirectToAction("AdmissionList"); // Adjust this to the appropriate view
        }

        // GET: Admissions/Index
        public ActionResult AdmissionList()
        {
            var admissions = dbContext.Admissions
                .Include("PatientProfile")
                .Include("Ward")
                .ToList();

            return View(admissions);
        }

        // GET: Admissions/Details/{id}
        //public IActionResult Details(int id)
        //{
        //    var admission = dbContext.Admissions
        //        .Include(a => a.PatientProfile)
        //        .Include(a => a.Ward)
        //        .FirstOrDefault(a => a.AdmissionId == id);

        //    if (admission == null)
        //    {
        //        return NotFound();
        //    }

        //    // Optionally, retrieve patient allergies and conditions
        //    var allergies = dbContext.patientAllergies
        //        .Where(pa => pa.PatientProfileId == admission.PatientProfileId)
        //        .ToList();

        //    var conditions = dbContext.PatientConditions
        //        .Where(pc => pc.PatientProfileId == admission.PatientProfileId)
        //        .ToList();

        //    ViewBag.Allergies = allergies;
        //    ViewBag.Conditions = conditions;

        //    return View(admission);
        //}
        // GET: Admissions/Details/{id}
        public IActionResult Details(int id)
        {
            // Retrieve the admission with related data
            var admission = dbContext.Admissions
                .Include(a => a.PatientProfile)
                .Include(a => a.Ward)
                .FirstOrDefault(a => a.AdmissionId == id);

            if (admission == null)
            {
                return NotFound();
            }

            // Retrieve patient allergies with active ingredient details
            var allergies = dbContext.patientAllergies
                .Where(pa => pa.PatientProfileId == admission.PatientProfileId)
                .Include(pa => pa.ActiveIngredients) // Include ActiveIngredient for its name
                .ToList();

            // Retrieve patient conditions with the related Condition details
            var conditions = dbContext.PatientConditions
                .Where(pc => pc.PatientProfileId == admission.PatientProfileId)
                .Include(pc => pc.Condition) // Include the Condition details
                .ToList();

            // Retrieve patient's vital signs
            var vitalSigns = dbContext.VitalSigns
                .Where(vs => vs.PatientProfileId == admission.PatientProfileId)
                .OrderByDescending(vs => vs.RecordedAt) // Latest records first
                .ToList();



            // Use ViewBag to pass allergies and conditions to the view
            ViewBag.Allergies = allergies;
            ViewBag.Conditions = conditions;
            ViewBag.VitalSigns = vitalSigns;

            return View(admission);
        }


    }

}

