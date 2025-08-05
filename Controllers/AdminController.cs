using E_PRESCRIBING_SYSTEM.Data;
using E_PRESCRIBING_SYSTEM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace E_PRESCRIBING_SYSTEM.Controllers
{
    public class AdminController : Controller
    {
        //private readonly ApplicationDbContext dbContext;
        //public AdminController(ApplicationDbContext dBD)
        //{
        //    dbContext = dBD;
        //}

        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext dBD, UserManager<IdentityUser> userManager)
        {
            dbContext = dBD;
            _userManager = userManager;
        }


        [HttpGet]
        public IActionResult AddChronicMedication()
        {
            // Populate dosage form dropdown
            var dosageForms = dbContext.Dosages
                .Select(d => new SelectListItem
                {
                    Value = d.DosageFormID.ToString(),
                    Text = d.DosageFormName
                })
                .OrderBy(d => d.Text)
                .ToList();
            ViewBag.DosageFormID = new SelectList(dosageForms, "Value", "Text");

            // Populate active ingredients dropdown
            ViewBag.ActiveIngredientsID = new SelectList(
                dbContext.ActiveIngredients.OrderBy(ai => ai.ActiveIngredientsName),
                "ActiveIngredientsID",
                "ActiveIngredientsName"
            );

            return View();
        }

      //  [HttpPost]
    //    public async Task<IActionResult> AddChronicMedication(
    //ChronicMedication chronicMedication,
    //List<int> selectedIngredients,
    //List<string> ingredientStrengths)
    //    {
    //        // Add chronic medication
    //        dbContext.ChricMedication.Add(chronicMedication);
    //        await dbContext.SaveChangesAsync();

    //        // Add active ingredients with strengths
    //        if (selectedIngredients != null && ingredientStrengths != null)
    //        {
    //            for (int i = 0; i < selectedIngredients.Count; i++)
    //            {
    //                var activeIngredient = new ActiveMedicationIngredient
    //                {
    //                    ChronicMedicationId = chronicMedication.ChronicMedicationId,
    //                    ActiveIngredientsID = selectedIngredients[i],
    //                    Strength = ingredientStrengths[i]
    //                };
    //                dbContext.ActiveMedicationIngredients.Add(activeIngredient);
    //            }
    //            await dbContext.SaveChangesAsync();
    //        }

    //        return RedirectToAction("ChronicMedicationList");
    //    }




        //[HttpGet]
        //public IActionResult ChronicMedicationList()
        //{
        //    var chronicMedications = dbContext.ChricMedication
        //        .Include(cm => cm.DosageForm) // Include Dosage Form
        //        .Include(cm => cm.MedicationActiveIngredients)
        //        .ThenInclude(ai => ai.ActiveIngredients)
        //        .Select(cm => new ChronicMedsViewModel
        //        {
        //            ChronicMedicationId = cm.ChronicMedicationId,
        //            ChronicMedicationName = cm.ChronicMedicationName,
        //            Schedule = cm.Schedule,
        //            DosageFormName = cm.DosageForm.DosageFormName,
        //            ActiveIngredients = cm.MedicationActiveIngredients
        //                .Select(ai => ai.ActiveIngredients.ActiveIngredientsName + " (" + ai.Strength + ")").ToList()
        //        })
        //        .OrderBy(cm => cm.ChronicMedicationName)
        //        .ToList();

        //    return View(chronicMedications);
        //}





        public IActionResult Index()
        {
            return View();
        }
        //public async Task<IActionResult> AdminDashboard()
        //{
        //    //var users = _userManager.Users.ToList(); // Get all registered users
        //    //return View(users); // Pass to view
        //    var users = _userManager.Users.ToList();
        //    var model = new List<UserWithRoleViewModel>();

        //    foreach (var user in users)
        //    {
        //        var roles = await _userManager.GetRolesAsync(user);
        //        model.Add(new UserWithRoleViewModel
        //        {
        //            UserName = user.UserName,
        //            Email = user.Email,
        //            Roles = roles
        //        });
        //    }

        //    //return View(model);



        //    var roleCounts = new Dictionary<string, int>
        //{
        //    { "Doctor", 0 },
        //    { "Nurse", 0 },
        //    { "Pharmacist", 0 },
        //    { "Admin", 0 }
        //};

        //    foreach (var user in users)
        //    {
        //        var roles = await _userManager.GetRolesAsync(user);
        //        foreach (var role in roles)
        //        {
        //            if (roleCounts.ContainsKey(role))
        //            {
        //                roleCounts[role]++;
        //            }
        //        }
        //    }

        //    ViewBag.RoleCounts = roleCounts;
        //    return View(users);


        //}

        public async Task<IActionResult> AdminDashboard()
        {
            var users = _userManager.Users.ToList();
            var model = new List<UserWithRoleViewModel>();

            var roleCounts = new Dictionary<string, int>
    {
        { "Doctor", 0 },
        { "Nurse", 0 },
        { "Pharmacist", 0 },
        { "Admin", 0 },
        { "Surgeon", 0 } // <-- Add Surgeon if it's a valid role
    };

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                model.Add(new UserWithRoleViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles
                });

                foreach (var role in roles)
                {
                    if (roleCounts.ContainsKey(role))
                    {
                        roleCounts[role]++;
                    }
                }
            }

            ViewBag.RoleCounts = roleCounts;
            return View(model); // <-- FIXED
        }


        public IActionResult AddActive()
        {
            IEnumerable<ActiveIngredients> actives = dbContext.ActiveIngredients;
            return View(actives);
        }
        public IActionResult AddIngredient(ActiveIngredients activeIngredient)
        {
            dbContext.ActiveIngredients.Add(activeIngredient);
            dbContext.SaveChanges();
            return RedirectToAction("AddActive");
        }
        public ActionResult Search(string searchText)
        {
            var results = dbContext.ActiveIngredients
                .Where(m => m.ActiveIngredientsName.Contains(searchText))
                .ToList();

            return PartialView("_SearchResults", results);
        }

        //DosageForm
        public IActionResult DosageForm()
        {
            IEnumerable<DosageForm> dosage = dbContext.Dosages;
            return View(dosage);
        }
        public IActionResult AddDosageForm(DosageForm dosageForm)
        {
            dbContext.Dosages.Add(dosageForm);
            dbContext.SaveChanges();
            return RedirectToAction("DosageForm");
        }
        public ActionResult SearchDosageForm(string searchText)
        {
            var results = dbContext.Dosages
                .Where(m => m.DosageFormName.Contains(searchText))
                .ToList();

            return PartialView("_SearchDosageForm", results);
        }


      

        public IActionResult CityList()
        {
            // Retrieve cities along with their provinces from the database
            var cities = dbContext.Citys
                .Include(c => c.Province)  // Ensure the Province is included
                .ToList();

            // Return the cities to the view
            return View(cities);
        }




        //Condition
        public IActionResult Condition()
        {
            IEnumerable<Condition> condition = dbContext.conditions;
            return View(condition);
        }
        public IActionResult AddCondition(Condition condition)
        {
            dbContext.conditions.Add(condition);
            dbContext.SaveChanges();
            return RedirectToAction("Condition");
        }
        public ActionResult SearchCondition(string searchText)
        {
            var result = dbContext.conditions
                .Where(m => m.Diagnosis.Contains(searchText))
                .ToList();

            return PartialView("_SearchCondition", result);
        }

        // GET: Load the update form into the modal
        public IActionResult UpdateCondition(int id)
        {
            var condition = dbContext.conditions.Find(id);
            if (condition == null)
            {
                return NotFound();
            }
            return PartialView("_UpdateConditionPartial", condition);
        }

        // POST: Update condition (without using ModelState.IsValid)
        [HttpPost]
        public IActionResult UpdateCondition(Condition condition)
        {
            var existingCondition = dbContext.conditions.Find(condition.ConditionID);
            if (existingCondition != null)
            {
                existingCondition.Diagnosis = condition.Diagnosis;
                dbContext.SaveChanges();
            }
            return Json(new { success = true });
        }

        public ActionResult Delete(int id)
        {
            // Fetch the condition based on the id
            var condition = dbContext.conditions.Find(id);
            if (condition == null)
            {
                return NotFound();
            }

            // Pass the condition to the view for confirmation
            return PartialView("_DeleteConfirmation", condition);  // For the modal confirmation
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var condition = dbContext.conditions.Find(id);
            if (condition != null)
            {
                // Remove the condition from the database
                dbContext.conditions.Remove(condition);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Condition");  // Redirect to the condition listing page
        }










        //Theatre
        public IActionResult Theatre()
        {
            IEnumerable<Threatre> threatres = dbContext.threatres;
            return View(threatres);
        }
        public IActionResult AddTheatre(Threatre threatre)
        {
            dbContext.threatres.Add(threatre);
            dbContext.SaveChanges();
            return RedirectToAction("Theatre");
        }
        public ActionResult SearchThreatre(string searchText)
        {
            var res = dbContext.threatres
                .Where(m => m.TheatreName.Contains(searchText))
                .ToList();

            return PartialView("_SearchThreatre", res);
        }

        //Treatment code
        public IActionResult TreatmentCode()
        {
            IEnumerable<TreatmentCode> condition = dbContext.TreatmentCodes;
            return View(condition);
        }
        public IActionResult AddTreatmentCode(TreatmentCode treatmentCode)
        {
            dbContext.TreatmentCodes.Add(treatmentCode);
            dbContext.SaveChanges();
            return RedirectToAction("TreatmentCode");
        }
        public ActionResult SearchTreatmentCode(string searchText)
        {
            var results = dbContext.TreatmentCodes
                .Where(m => m.Code.Contains(searchText))
                .ToList();

            return PartialView("_SearchTreatmentCode", results);
        }

        [HttpGet]
        public IActionResult GetTreatmentById(int id)
        {
            var treatment = dbContext.TreatmentCodes.FirstOrDefault(x => x.TreatmentID == id);
            if (treatment == null)
            {
                return NotFound();
            }
            return Json(treatment);
        }


        [HttpPost]
        public IActionResult UpdateTreatment(TreatmentCode treatmentCode)
        {
            var existingTreatment = dbContext.TreatmentCodes.FirstOrDefault(x => x.TreatmentID == treatmentCode.TreatmentID);
            if (existingTreatment != null)
            {
                existingTreatment.Code = treatmentCode.Code;
                existingTreatment.Description = treatmentCode.Description;
                dbContext.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult DeleteTreatment(int TreatmentID)
        {
            var treatment = dbContext.TreatmentCodes.Find(TreatmentID);
            if (treatment != null)
            {
                dbContext.TreatmentCodes.Remove(treatment);
                dbContext.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }






        //Ward
        public IActionResult Ward()
        {
            IEnumerable<Ward> wards = dbContext.Wards;
            return View(wards);
        }
        public IActionResult AddWard(Ward ward)
        {
            dbContext.Wards.Add(ward);
            dbContext.SaveChanges();
            return RedirectToAction("Ward");
        }
        public ActionResult SearchWard(string searchText)
        {
            var results = dbContext.Wards
                .Where(m => m.WardName.Contains(searchText))
                .ToList();

            return PartialView("_SearchWard", results);
        }

        //Province
        public IActionResult Province()
        {
            IEnumerable<Province> province = dbContext.Provinces;
            return View(province);
        }
        public IActionResult AddProvince(Province province)
        {
            dbContext.Provinces.Add(province);
            dbContext.SaveChanges();
            return RedirectToAction("Province");
        }
        public ActionResult SearchProvince(string searchText)
        {
            var res = dbContext.Provinces
                .Where(m => m.ProvinceName.Contains(searchText))
                .ToList();

            return PartialView("_SearchProvince", res);
        }

        //City
        public ActionResult City()
        {
            var viewModel = new CityViewModel
            {
                Provinces = dbContext.Provinces.Select(p => new SelectListItem
                {
                    Value = p.ProvinceID.ToString(),
                    Text = p.ProvinceName
                })
            };

            return View(viewModel);
        }
        [HttpPost]
        public ActionResult City(CityViewModel model)
        {
            // Manual validation
            if (string.IsNullOrEmpty(model.CityName))
            {
                // You can add error messages or handle it as per your requirements
                ViewBag.ErrorMessage = "City name is required";
                model.Provinces = dbContext.Provinces.Select(p => new SelectListItem
                {
                    Value = p.ProvinceID.ToString(),
                    Text = p.ProvinceName
                });
                return View(model);
            }

            if (model.ProvinceID <= 0)
            {
                ViewBag.ErrorMessage = "Please select a valid province";
                model.Provinces = dbContext.Provinces.Select(p => new SelectListItem
                {
                    Value = p.ProvinceID.ToString(),
                    Text = p.ProvinceName
                });
                return View(model);
            }

            // If validation passes, create a new City entity
            var city = new City
            {
                CityName = model.CityName,
                ProvinceID = model.ProvinceID
            };

            // Add and save to the database
            dbContext.Citys.Add(city);
            dbContext.SaveChanges();

            // Redirect or return success message
            return RedirectToAction("City");
        }

        public ActionResult SearchCity(string searchText)
        {
            var res = dbContext.Citys
                .Where(m => m.CityName.Contains(searchText))
                .ToList();

            return PartialView("_SearchCity", res);
        }

        //suburb



        // GET: Display the form to add a suburb
        public IActionResult AddSuburb()
        {
            var viewModel = new SuburbViewModel
            {
                Cities = dbContext.Citys.Select(c => new SelectListItem
                {
                    Value = c.CityID.ToString(),
                    Text = c.CityName
                }).ToList()
            };
            return View(viewModel);
        }




        [HttpPost]
        public IActionResult AddSuburb(SuburbViewModel viewModel)
        {
            // Custom validation logic
            if (string.IsNullOrEmpty(viewModel.SuburbName) || string.IsNullOrEmpty(viewModel.PostalCode) || viewModel.CityID == 0)
            {
                // Handle validation errors
                ViewBag.ErrorMessage = "All fields are required.";

                // Reload the city dropdown list
                viewModel.Cities = dbContext.Citys.Select(c => new SelectListItem
                {
                    Value = c.CityID.ToString(),
                    Text = c.CityName
                }).ToList();

                return View(viewModel); // Return the view with the error message
            }

            // If validation is successful, save the suburb
            var suburb = new Suburb
            {
                SuburbName = viewModel.SuburbName,
                PostalCode = viewModel.PostalCode,
                CityID = viewModel.CityID
            };

            dbContext.Suburbs.Add(suburb);
            dbContext.SaveChanges();

            return RedirectToAction("SuburbList");
        }

        public IActionResult SuburbList()
        {
            var suburbs = dbContext.Suburbs.Include(s => s.City).ToList();
            return View(suburbs);
        }


        // GET: Medications/Create
        public ActionResult Create()
        {
            // Get the list of active ingredients from the database
            ViewBag.ActiveIngredientsID = new SelectList(dbContext.ActiveIngredients, "ActiveIngredientsID", "ActiveIngredientsName");

            // Get the list of dosage forms from the database
            ViewBag.DosageFormID = new SelectList(dbContext.Dosages, "DosageFormID", "DosageFormName");

            return View();
            //var model = new MedicationViewModel
            //{
            //    ActiveIngredients = new List<ActiveIngredientViewModel>() // Initialize to prevent null reference
            //};

            //ViewBag.ActiveIngredients = GetActiveIngredients(); // Populate your active ingredients here
            //return View(model);


        }
        private IEnumerable<ActiveIngredientViewModel> GetActiveIngredients()
        {
            // Retrieve active ingredients from the database
            // This example assumes you have an ActiveIngredient entity
            return dbContext.ActiveIngredients
                .Select(ingredient => new ActiveIngredientViewModel
                {
                    ActiveIngredientsID = ingredient.ActiveIngredientsID,
                    ActiveIngredientsName = ingredient.ActiveIngredientsName
                })
                .ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Medication medication)
        {
            // Handle the case where medication is null
            if (medication == null)
            {
                // Set an error message for the user or log the error
                ViewBag.Error = "No medication data was provided. Please fill in the form.";

                // Repopulate the ViewBag to ensure the dropdowns are available
                ViewBag.ActiveIngredientsID = new SelectList(dbContext.ActiveIngredients, "ActiveIngredientsID", "ActiveIngredientsName");
                ViewBag.DosageFormID = new SelectList(dbContext.Dosages, "DosageFormID", "DosageFormName");

                // Return the view with an empty or default medication model to avoid null issues in the view
                return View(new Medication());
            }

            // Manually validate the model
            if (string.IsNullOrEmpty(medication.MedicationName) || medication.ActiveIngredientsID == 0 || medication.DosageFormID == 0)
            {
                ViewBag.Error = "Please fill in all required fields.";

                // Repopulate the ViewBag in case of validation failure
                ViewBag.ActiveIngredientsID = new SelectList(dbContext.ActiveIngredients, "ActiveIngredientsID", "ActiveIngredientsName", medication.ActiveIngredientsID);
                ViewBag.DosageFormID = new SelectList(dbContext.Dosages, "DosageFormID", "DosageFormName", medication.DosageFormID);

                return View(medication);
            }

            // Save data if valid
            dbContext.Medications.Add(medication);
            dbContext.SaveChanges();
            return RedirectToAction("Create");
        }


        //Add medication with multiple active
        [HttpGet]
        public IActionResult AddMedication()
        {
            // Populate dosage form dropdown
            // ViewBag.DosageFormID = new SelectList(dbContext.Dosages, "DosageFormID", "DosageFormName");
            var dosageForms = dbContext.Dosages
                            .Select(d => new SelectListItem
                            {
                                Value = d.DosageFormID.ToString(),
                                Text = d.DosageFormName
                            })
                            .OrderBy(d => d.Text) // Sort by the text (Dosage Form name)
                            .ToList();

            ViewBag.DosageFormID = new SelectList(dosageForms, "Value", "Text");


            // Populate active ingredient dropdown (assuming the ingredients come from a database)
            //  ViewBag.ActiveIngredientsID = new SelectList(dbContext.ActiveIngredients, "ActiveIngredientsID", "ActiveIngredientsName");
            ViewBag.ActiveIngredientsID = new SelectList(
      dbContext.ActiveIngredients.OrderBy(ai => ai.ActiveIngredientsName),
      "ActiveIngredientsID",
      "ActiveIngredientsName"
  );


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMedication(PharmacyMedication pharmacyMedication, List<int> selectedIngredients, List<string> ingredientStrengths)
        {
            dbContext.PharmacyMedication.Add(pharmacyMedication);
            await dbContext.SaveChangesAsync();

            //Add Ingredients
            if (selectedIngredients != null && ingredientStrengths != null)
            {
                for (int i = 0; i < selectedIngredients.Count; i++)
                {
                    var ingredient = new ActiveMedicationIngredient
                    {
                        PharmacyMedicationId = pharmacyMedication.PharmacyMedicationId,
                        ActiveIngredientsID = selectedIngredients[i],
                        Strength = ingredientStrengths[i]
                    };
                    dbContext.ActiveMedicationIngredients.Add(ingredient);
                }
                await dbContext.SaveChangesAsync();
            }
            // Populate dosage form dropdown
            ViewBag.DosageFormID = new SelectList(dbContext.Dosages, "DosageFormID", "DosageFormName");

            // Populate active ingredient dropdown (assuming the ingredients come from a database)
            ViewBag.ActiveIngredientsID = new SelectList(dbContext.ActiveIngredients, "ActiveIngredientsID", "ActiveIngredientsName");

            return RedirectToAction("ChonicMedicationList");
        }






    }




} 

