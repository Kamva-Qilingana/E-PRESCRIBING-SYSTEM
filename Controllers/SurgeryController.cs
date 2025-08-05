using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E_PRESCRIBING_SYSTEM.Models;
using Newtonsoft.Json;
using E_PRESCRIBING_SYSTEM.Data;
using E_PRESCRIBING_SYSTEM.ViewModels;

namespace E_PRESCRIBING_SYSTEM.Controllers
{
    public class SurgeryController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public SurgeryController(ApplicationDbContext context)
        {
            dbContext = context;
        }
        [HttpGet]
        public IActionResult Index(string searchQuery)
        {
            // Filter surgeries based on the search query
            var surgeries = string.IsNullOrWhiteSpace(searchQuery)
                ? dbContext.Surgery.Include(s => s.PatientProfile)
                                   .Include(s => s.Theatre)
                                   .Include(s => s.SurgeryTreatments)
                                       .ThenInclude(st => st.TreatmentCode)
                                   .ToList()
                : dbContext.Surgery.Include(s => s.PatientProfile)
                                   .Include(s => s.Theatre)
                                   .Include(s => s.SurgeryTreatments)
                                       .ThenInclude(st => st.TreatmentCode)
                                   .Where(s => s.PatientProfile.PatientIDno.Contains(searchQuery) ||
                                               s.PatientProfile.PatientName.Contains(searchQuery))
                                   .ToList();

            ViewBag.SearchQuery = searchQuery; // Pass search query to the view

            return View(surgeries);
        }

        public IActionResult Create(string searchQuery)
        {
            // Filter patients based on the search query, if provided
            var patients = string.IsNullOrWhiteSpace(searchQuery)
                ? dbContext.PatientProfiles.ToList()
                : dbContext.PatientProfiles
                    .Where(p => p.PatientIDno.Contains(searchQuery) || p.PatientName.Contains(searchQuery)) // Adjust field names as necessary
                    .ToList();

            var model = new SurgeryViewModel
            {
                Patients = patients,
                Theatres = dbContext.threatres.ToList(),
                Treatments = dbContext.TreatmentCodes.ToList()
            };

            ViewBag.SearchQuery = searchQuery; // Pass the search query to the view
            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SurgeryViewModel model)
        {
           
                var surgery = new Surgery
                {
                    PatientProfileId = model.PatientProfileId,
                    TheatreID = model.TheatreID,
                    SurgeryDate = model.SurgeryDate,
                    TimeSlot = model.TimeSlot,
                    CreatedBy = User.Identity.Name,
                    SurgeryTreatments = model.SelectedTreatmentIDs.Select(tid => new SurgeryTreatment
                    {
                        TreatmentID = tid
                    }).ToList()
                };

                dbContext.Surgery.Add(surgery);
                dbContext.SaveChanges();

            

            // If model state is invalid, repopulate dropdowns
            model.Patients = dbContext.PatientProfiles.ToList();
            model.Theatres = dbContext.threatres.ToList();
            model.Treatments = dbContext.TreatmentCodes.ToList();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Retrieve the surgery details by ID
            var surgery = dbContext.Surgery
                .Include(s => s.PatientProfile)
                .Include(s => s.Theatre)
                .Include(s => s.SurgeryTreatments)
                    .ThenInclude(st => st.TreatmentCode)
                .FirstOrDefault(s => s.SurgeryID == id);

            if (surgery == null)
            {
                return NotFound();
            }

            // Prepare the view model
            var viewModel = new SurgeryViewModel
            {
                SurgeryID = surgery.SurgeryID,
                PatientProfileId = surgery.PatientProfileId,
                TheatreID = surgery.TheatreID,
                SurgeryDate = surgery.SurgeryDate,
                TimeSlot = surgery.TimeSlot,
                SelectedTreatmentIDs = surgery.SurgeryTreatments.Select(st => st.TreatmentID).ToList(),
                Patients = dbContext.PatientProfiles.ToList(),
                Theatres = dbContext.threatres.ToList(),
                Treatments = dbContext.TreatmentCodes.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SurgeryViewModel model)
        {
           
                // Find the existing surgery
                var surgery = dbContext.Surgery
                    .Include(s => s.SurgeryTreatments)
                    .FirstOrDefault(s => s.SurgeryID == model.SurgeryID);

                if (surgery == null)
                {
                    return NotFound();
                }

                // Update the surgery details
                surgery.PatientProfileId = model.PatientProfileId;
                surgery.TheatreID = model.TheatreID;
                surgery.SurgeryDate = model.SurgeryDate;
                surgery.TimeSlot = model.TimeSlot;

                // Update treatments
                surgery.SurgeryTreatments.Clear();
                if (model.SelectedTreatmentIDs != null)
                {
                    surgery.SurgeryTreatments = model.SelectedTreatmentIDs.Select(tid => new SurgeryTreatment
                    {
                        TreatmentID = tid,
                        SurgeryID = surgery.SurgeryID
                    }).ToList();
                }

                dbContext.SaveChanges();
              //  return RedirectToAction("Index");
            

            // If the model is invalid, repopulate dropdowns
            model.Patients = dbContext.PatientProfiles.ToList();
            model.Theatres = dbContext.threatres.ToList();
            model.Treatments = dbContext.TreatmentCodes.ToList();

            return RedirectToAction("Index");
        }


        // GET: Delete Surgery
        public IActionResult Delete(int id)
        {
            var surgery = dbContext.Surgery
                .Include(s => s.PatientProfile)
                .Include(s => s.Theatre)
               // .Include(s => s.TreatmentCode)
                .FirstOrDefault(s => s.SurgeryID == id);

            if (surgery == null)
            {
                return NotFound();
            }

            return View(surgery);
        }

        // POST: Delete Surgery
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var surgery = dbContext.Surgery
                .Where(s => s.SurgeryID == id)
                .FirstOrDefault();

            if (surgery == null)
            {
                return NotFound();
            }

            dbContext.Surgery.Remove(surgery);
            dbContext.SaveChanges();

            return RedirectToAction("Index"); // Redirect to a suitable action after deletion
        }

        public ActionResult SearchSurgery(string searchText)
        {
            // Check if searchText is empty or null
            if (string.IsNullOrEmpty(searchText))
            {
                return PartialView("_SearchResults", new List<Surgery>()); // Return empty view if no search text is provided
            }

            // Query surgeries by patient ID number (PatientIDno)
            var results = dbContext.Surgery
                .Include(s => s.PatientProfile) // Include related PatientProfile for access to PatientIDno
                .Where(s => s.PatientProfile.PatientIDno.Contains(searchText)) // Filter by PatientIDno
                .ToList();

            return PartialView("_SearchSurgeryResults", results); // Return a partial view with the search results
        }




        public IActionResult SurgeryReport(DateTime? startDate, DateTime? endDate)
        {
            // Set default date range if not provided
            var defaultStartDate = DateTime.Now.AddMonths(-1);
            var defaultEndDate = DateTime.Now;

            var reportStartDate = startDate ?? defaultStartDate;
            var reportEndDate = endDate ?? defaultEndDate;

            var surgeries = dbContext.Surgery
                .Include(s => s.PatientProfile)
                .Include(s => s.SurgeryTreatments)
                    .ThenInclude(st => st.TreatmentCode)
                .Where(s => s.SurgeryDate >= reportStartDate && s.SurgeryDate <= reportEndDate)
                .ToList();

            var surgeryData = surgeries.Select(s => new SurgeryData
            {
                SurgeryDate = s.SurgeryDate,
                PatientName = s.PatientProfile.PatientIDno, // Assuming `FullName` exists in `PatientProfile`
                TreatmentCodes = s.SurgeryTreatments.Select(st => st.TreatmentCode.Code).ToList() // Assuming `Code` exists in `TreatmentCode`
            }).ToList();

            var treatmentCodeSummary = surgeries
                .SelectMany(s => s.SurgeryTreatments)
                .GroupBy(st => st.TreatmentCode.Code)
                .ToDictionary(g => g.Key, g => g.Count());

            var viewModel = new SurgeryReportViewModel
            {
                StartDate = reportStartDate,
                EndDate = reportEndDate,
                SurgeryList = surgeryData,
                TotalPatients = surgeries.Select(s => s.PatientProfileId).Distinct().Count(),
                TreatmentCodeSummary = treatmentCodeSummary
            };

            return View(viewModel);
        }






        //public IActionResult SurgeryReport(DateTime? startDate, DateTime? endDate)
        //{
        //    // Default date range if not specified
        //    if (!startDate.HasValue) startDate = DateTime.Now.AddMonths(-1);
        //    if (!endDate.HasValue) endDate = DateTime.Now;

        //    // Query surgeries within the date range
        //    var surgeries = dbContext.Surgery
        //        .Include(s => s.PatientProfile)
        //      //  .Include(s => s.TreatmentCode)
        //        .Where(s => s.SurgeryDate >= startDate && s.SurgeryDate <= endDate)
        //        .ToList();

        //    // Create the report data
        //    var surgeryDataList = new List<SurgeryData>();
        //    var treatmentCodeSummary = new Dictionary<string, int>();

        //    foreach (var surgery in surgeries)
        //    {
        //        // Collect surgery data
        //        var surgeryData = new SurgeryData
        //        {
        //            SurgeryDate = surgery.SurgeryDate,
        //            PatientName = surgery.PatientProfile.PatientName, // Assuming PatientProfile has FullName
        //            TreatmentCodes = new List<string> { surgery.TreatmentCode.Description } // Assuming Code is the treatment code
        //        };

        //        // Add to the surgery data list
        //        surgeryDataList.Add(surgeryData);

        //        // Summarize treatment codes
        //        if (!treatmentCodeSummary.ContainsKey(surgery.TreatmentCode.Description))
        //        {
        //            treatmentCodeSummary[surgery.TreatmentCode.Description] = 0;
        //        }
        //        treatmentCodeSummary[surgery.TreatmentCode.Description]++;
        //    }

        //    // Create the ViewModel
        //    var reportViewModel = new SurgeryReportViewModel
        //    {
        //        StartDate = startDate.Value,
        //        EndDate = endDate.Value,
        //        SurgeryList = surgeryDataList,
        //        TotalPatients = surgeryDataList.Select(s => s.PatientName).Distinct().Count(), // Count distinct patients
        //        TreatmentCodeSummary = treatmentCodeSummary
        //    };

        //    return View(reportViewModel);
        //}

    }
}
