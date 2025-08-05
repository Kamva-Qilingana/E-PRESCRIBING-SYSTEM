using E_PRESCRIBING_SYSTEM.Data;
using E_PRESCRIBING_SYSTEM.Models;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace E_PRESCRIBING_SYSTEM.Controllers
{
    public class PharmacistController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfiguration _configuration;

        public PharmacistController(ApplicationDbContext dBD, IConfiguration configuration)
        {
            dbContext = dBD;
            _configuration = configuration;
        }


       

        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult PharmacistDashboard()
        //{
        //    // Get medication names and stock quantities
        //    var medicationNames = dbContext.PharmacyMedication
        //        .Select(m => m.Name)
        //        .ToList();

        //    var stockQuantities = dbContext.PharmacyMedication
        //        .Select(m => m.StockOnHand)
        //        .ToList();

        //    ViewBag.MedicationNames = medicationNames;
        //    ViewBag.StockQuantities = stockQuantities;

        //    return View();
        //}

        public IActionResult PharmacistDashboard()
        {
            // Chart data for stock on hand
            var medicationNames = dbContext.PharmacyMedication
                .Select(m => m.Name)
                .ToList();

            var stockQuantities = dbContext.PharmacyMedication
                .Select(m => m.StockOnHand)
                .ToList();

            ViewBag.MedicationNames = medicationNames;
            ViewBag.StockQuantities = stockQuantities;

            // Dashboard card counts
            ViewBag.TotalMedications = dbContext.PharmacyMedication.Count();
            ViewBag.TotalPrescriptions = dbContext.NewPrescriptions.Count();
            ViewBag.TotalOrders = dbContext.StockOrders.Count(); // Adjust table if named differently

            return View();
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
            if(selectedIngredients != null && ingredientStrengths != null)
            {
                for(int i = 0; i < selectedIngredients.Count; i++)
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

            return RedirectToAction("MedicationList");
        }

      


        // Action to process the order
        [HttpPost]
        public IActionResult OrderStock(MedicationOrderViewModel model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", "Model is null.");
                return View(model);
            }
            foreach (var item in model.Medications.Where(m => m.IsSelected && m.OrderQuantity > 0))
            {
                var Order = new OrderStock
                {
                    //    StockID = item.DayMedicationId,
                    PharmacyMedicationId = item.PharmacyMedicationId,
                    Date = DateTime.Now,
                    OrderQuantity = item.OrderQuantity,
                    Status = "Ordered"
                };

                dbContext.OrderStock.Add(Order);
                dbContext.SaveChanges();
            }

            //dbContext.SaveChanges();


            return RedirectToAction("OrderStock");
        }

        [HttpGet]


        // GET: Pharmacist/GetStock
        public IActionResult GetStock()
        {
            var stockOrders = dbContext.OrderStock
                .Include(s => s.PharmacyMedication) // Include related medication data
                .Select(s => new StockOrderViewModel
                {
                    StockID = s.StockID,
                    Name = s.PharmacyMedication.Name,
                    OrderQuantity = s.OrderQuantity,
                    Date = s.Date,
                    Status = s.Status
                }).ToList();

            var viewModel = new MedicationOrderViewModel
            {
                StockOrders = stockOrders
            };

            return View(viewModel); // Pass the view model to the view
        }

        // GET: Pharmacist/EditOrderStock/5
        public IActionResult EditOrderStock(int id)
        {
            var orderStock = dbContext.StockOrders
                .Include(s => s.PharmacyMedication)  // Assuming you have a related medication entity
                .FirstOrDefault(s => s.StockOrderID == id);

            if (orderStock == null)
            {
                return NotFound();
            }

            // Convert the OrderStock entity to the StockOrderViewModel
            var viewModel = new OrderStockViewModel
            {
                StockOrderId = orderStock.StockOrderID,
                Name = orderStock.PharmacyMedication.Name,  // Assuming PharmacyMedication has the Name property
                OrderQuantity = orderStock.OrderQuantity,
                Date = orderStock.Date,
                Status = orderStock.Status
            };

            // Pass the correct view model to the view
            return View(viewModel);
        }

        [HttpPost]
         //[ValidateAntiForgeryToken]
          public IActionResult EditOrderStock(OrderStockViewModel model)
          {
   
              var orderStock = dbContext.StockOrders.FirstOrDefault(s => s.StockOrderID == model.StockOrderId);

               if (orderStock == null)
               {
                   return NotFound();
               }

               //      Update the order stock entity with values from the view model
                 orderStock.OrderQuantity = model.OrderQuantity;
                 orderStock.Status = model.Status;
        
                 orderStock.Date = model.Date;  // Optional

                 dbContext.Update(orderStock);
                 dbContext.SaveChanges();

                return RedirectToAction("GetStockOrder");
    

                 //return View(model);
          }




        public IActionResult DeleteOrderStock(int id)
        {
            var orderStock = dbContext.StockOrders.FirstOrDefault(s => s.StockOrderID == id);
            if (orderStock == null)
            {
                return NotFound();
            }

            dbContext.StockOrders.Remove(orderStock);
            dbContext.SaveChanges();

            return RedirectToAction("GetStockOrder");
        }




        //RECEIVED STOCK
        // Display orders that are ready to be received
        // POST: ReceivedStock/Create

        // GET: ReceivedStock/Create
        public IActionResult Create()
        {
            // Load the orders to select from (you can modify this query as needed)
            var orders = dbContext.OrderStock
                .Include(o => o.PharmacyMedication)
                .ToList();

            ViewBag.Orders = new SelectList(orders, "StockID", "PharmacyMedication.Name"); // Change "PharmacyMedication.Name" based on your needs
            return View();
        }


        [HttpGet]
        public IActionResult AddPatientProfile()
        {
            //return View(new DEMO.Models.PatientProfile());

            IEnumerable<PatientProfile> patientProfiles = dbContext.PatientProfiles;
            return View(patientProfiles);
        }
        [HttpPost]
        public IActionResult AddPatientProfile(PatientProfile patientProfile)
        {
            dbContext.PatientProfiles.Add(patientProfile);
            dbContext.SaveChanges();
            return RedirectToAction("AddPatientProfile");
        }

        public ActionResult SearchPatientProfile(string searchText)
        {
            var res = dbContext.PatientProfiles
                .Where(m => m.PatientIDno.Contains(searchText))
                .ToList();

            return PartialView("_SearchPatientProfile", res);
        }


        [HttpGet]
        public IActionResult MedicationList()
        {
            var medications = dbContext.PharmacyMedication
                .Include(m => m.DosageForm) // Include DosageForm relationship
                .Include(m => m.MedicationActiveIngredients) // Include active ingredients
                .ThenInclude(ai => ai.ActiveIngredients) // Include actual active ingredient details
                .Select(m => new MedsViewModel
                {
                    PharmacyMedicationId = m.PharmacyMedicationId,
                    Name = m.Name,
                    Schedule = m.Schedule,
                    ReorderLevel = m.ReorderLevel,
                    StockOnHand = m.StockOnHand,
                    DosageFormName = m.DosageForm.DosageFormName, // Get the dosage form name
                    ActiveIngredients = m.MedicationActiveIngredients
                        .Select(ai => ai.ActiveIngredients.ActiveIngredientsName + " (" + ai.Strength + ")").ToList() // Get active ingredient names with strengths
                })
                .OrderBy(m => m.Name) // Sort the medications by Name in ascending order
                .ToList();

            return View(medications);
        }

        // GET: Pharmacist/EditMedication/5
        [HttpGet]
        public IActionResult EditMedication(int id)
        {
            var medication = dbContext.PharmacyMedication
                .Include(m => m.MedicationActiveIngredients)
                .FirstOrDefault(m => m.PharmacyMedicationId == id);

            if (medication == null)
            {
                return NotFound();
            }

            // Populate view models with data for editing
            ViewBag.DosageFormID = new SelectList(dbContext.Dosages, "DosageFormID", "DosageFormName", medication.DosageFormID);
            ViewBag.ActiveIngredientsID = new SelectList(dbContext.ActiveIngredients, "ActiveIngredientsID", "ActiveIngredientsName");


            return View(medication); // Return a view for editing
        }

        // POST: Pharmacist/EditMedication/5
        [HttpPost]
        public async Task<IActionResult> EditMedication(PharmacyMedication pharmacyMedication, List<int> selectedIngredients, List<string> ingredientStrengths)
        {
            // Update medication details and active ingredients in the database
            dbContext.Update(pharmacyMedication);
            await dbContext.SaveChangesAsync();

            // Logic for updating active ingredients would go here...

            return RedirectToAction("MedicationList");
        }

        // GET: Pharmacist/DeleteMedication/5
        [HttpGet]
        public async Task<IActionResult> DeleteMedication(int id)
        {
            var medication = await dbContext.PharmacyMedication.FindAsync(id);

            if (medication == null)
            {
                return NotFound();
            }

            // Remove the medication
            dbContext.PharmacyMedication.Remove(medication);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("MedicationList");
        }

        // GET: Patient/History/5
        public ActionResult History(int id)
        {
            // Fetch the patient profile
            var patientProfile = dbContext.PatientProfiles.Find(id);

            if (patientProfile == null)
            {
                // Redirect to an error page or display a custom message
                return RedirectToAction("Error", "Home", new { message = "Patient not found." });
            }

            // Fetch patient conditions (diagnosis)
            var conditions = dbContext.PatientConditions
                .Where(c => c.PatientProfileId == id)
                .Include(c => c.Condition)
                .ToList();

            // Fetch patient medications
            var medications = dbContext.PatientMedications
                .Where(m => m.PatientProfileId == id)
                .Include(m => m.PharmacyMedication)
                .ToList();

            // Fetch patient allergies
            var allergies = dbContext.patientAllergies
                .Where(a => a.PatientProfileId == id)
                .Include(a => a.ActiveIngredients)
                .ToList();

            // Create ViewModel with all patient history data
            var viewModel = new PatientHistoryViewModel
            {
                PatientProfileId = patientProfile.PatientProfileId,
                PatientIDno = patientProfile.PatientIDno,
                PatientName = patientProfile.PatientName,
                //BirthDate = patientProfile.BirthDate,
                Conditions = conditions,
                Medications = medications,
                Allergies = allergies
            };

            return View(viewModel);
        }

        // GET: Pharmacist/GetStock
        public IActionResult GetStockOrder()
        {
            var stockOrders = dbContext.StockOrders
                .Include(s => s.PharmacyMedication) // Include related medication data
                .Select(s => new OrderStockViewModel
                {
                    StockOrderId = s.StockOrderID,
                    Name = s.PharmacyMedication.Name,
                    OrderQuantity = s.OrderQuantity,
                    Date = s.Date,
                    Status = s.Status
                }).ToList();

            var viewModel = new StockViewModel
            {
                StockOrders = stockOrders
            };

            return View(viewModel); // Pass the view model to the view
        }

        // GET: Pharmacist/EditOrderStock/5
        public IActionResult EditStockOrder(int id)
        {
            var orderStock = dbContext.StockOrders
                .Include(s => s.PharmacyMedication)  // Assuming you have a related medication entity
                .FirstOrDefault(s => s.StockOrderID == id);

            if (orderStock == null)
            {
                return NotFound();
            }

            // Convert the OrderStock entity to the StockOrderViewModel
            var viewModel = new OrderStockViewModel
            {
                StockOrderId = orderStock.StockOrderID,
                Name = orderStock.PharmacyMedication.Name,  // Assuming PharmacyMedication has the Name property
                OrderQuantity = orderStock.OrderQuantity,
                Date = orderStock.Date,
                Status = orderStock.Status
            };

            // Pass the correct view model to the view
            return View(viewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult EditStockOrder(OrderStockViewModel model)
        {

            var orderStock = dbContext.StockOrders.FirstOrDefault(s => s.StockOrderID == model.StockOrderId);

            if (orderStock == null)
            {
                return NotFound();
            }

            //      Update the order stock entity with values from the view model
            orderStock.OrderQuantity = model.OrderQuantity;
            orderStock.Status = model.Status;

            orderStock.Date = model.Date;  // Optional

            dbContext.Update(orderStock);
            dbContext.SaveChanges();

            return RedirectToAction("GetStockOrder");


            //return View(model);
        }




        public IActionResult DeleteStockOrder(int id)
        {
            var orderStock = dbContext.OrderStock.FirstOrDefault(s => s.StockID == id);
            if (orderStock == null)
            {
                return NotFound();
            }

            dbContext.OrderStock.Remove(orderStock);
            dbContext.SaveChanges();

            return RedirectToAction("GetStock");
        }

        //Stock management
        // Action to display the order form
        [HttpGet]
        public IActionResult StockOrder()
        {
            var viewModel = new StockViewModel
            {
                Medications = dbContext.PharmacyMedication
                    .Where(m => m.Name != null && m.StockOnHand != null && m.ReorderLevel != null)
                    .Select(m => new PharmacyMedicationViewModel
                    {
                        PharmacyMedicationId = m.PharmacyMedicationId,
                        Name = m.Name,
                        StockOnHand = m.StockOnHand,
                        ReorderLevel = m.ReorderLevel
                    }).ToList(),

                StockOrders = dbContext.StockOrders
                    .Include(o => o.PharmacyMedication)
                    .Where(o => o.PharmacyMedication != null)
                    .Select(o => new OrderStockViewModel
                    {
                        StockOrderId = o.StockOrderID,
                        OrderQuantity = o.OrderQuantity,
                        Date = o.Date,
                        Status = o.Status
                    }).ToList()
            };

            return View(viewModel);
        }


        // Action to process the order
        [HttpPost]
        public async Task<IActionResult> StockOrder(StockViewModel model)
        {
            try
            {
                var emailService = new EmailService(_configuration);
                var purchaseManagerEmail = "qilinganakamva@gmail.com";

                var orderedMedications = new List<string>();

                if (model.Medications == null || !model.Medications.Any())
                {
                    TempData["ErrorMessage"] = "No medications selected for the stock order.";
                    return RedirectToAction("StockOrder");
                }

                foreach (var item in model.Medications.Where(m => m.IsSelected && m.OrderQuantity > 0))
                {
                    var pharmacyMedication = await dbContext.PharmacyMedication
                        .FirstOrDefaultAsync(m => m.PharmacyMedicationId == item.PharmacyMedicationId);

                    if (pharmacyMedication == null)
                    {
                        continue; // Skip if medication not found
                    }

                    var stockOrder = new StockOrder
                    {
                        PharmacyMedicationId = item.PharmacyMedicationId,
                        Date = DateTime.Now,
                        OrderQuantity = item.OrderQuantity,
                        Status = "Ordered"
                    };

                    dbContext.StockOrders.Add(stockOrder);

                    orderedMedications.Add($"{pharmacyMedication.Name} - Quantity: {item.OrderQuantity}");
                }

                await dbContext.SaveChangesAsync();

                if (orderedMedications.Any())
                {
                    var subject = "Stock Order Notification";
                    var message = $"Dear Purchase Manager,<br/><br/>The following medications have been ordered:<br/>" +
                                  string.Join("<br/>", orderedMedications) +
                                  "<br/><br/>Best regards,<br/>Pharmacy Team<br/></br/><small>Bay Breeze Day Hospital<br/>Eastern Cape<br/>Summerstrand (6001)<br/>1 8th Avenue<br/>041 58 2121</small >";

                    

                    await emailService.SendEmailAsync(purchaseManagerEmail, subject, message);
                }

                TempData["EmailMessage"] = "Stock order placed successfully and email notification sent.";
                return RedirectToAction("StockOrder");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("StockOrder");
            }
        }

      
         // Method to display the list of received stock
    [HttpGet]
    public IActionResult StockReceivedList()
    {
        // Retrieve all received stock from the database, including related StockOrder and PharmacyMedication
        var receivedStockList = dbContext.StockReceived
            .Include(sr => sr.StockOrder)
            .Include(sr => sr.PharmacyMedication)
            .Select(sr => new ReceivedViewModel
            {
                StockReceivedID = sr.StockReceivedID,
                StockOrderID = sr.StockOrderID,
                PharmacyMedicationId = sr.PharmacyMedicationId,
                MedicationName = sr.PharmacyMedication.Name,
                ReceivedQuantity = sr.ReceivedQuantity,
                ReceivedDate = sr.ReceivedDate,
                StockOrderStatus = sr.StockOrder.Status
            })
            .ToList();

        return View(receivedStockList);
    }
        // GET: Display the combined stock ordering and receiving page
        //[HttpGet]
        //public IActionResult ManageStock()
        //{
        //    var stockOrderModel = new StockViewModel
        //    {
        //        Medications = dbContext.PharmacyMedication
        //            .Where(m => m.Name != null && m.StockOnHand != null && m.ReorderLevel != null)
        //            .Select(m => new PharmacyMedicationViewModel
        //            {
        //                PharmacyMedicationId = m.PharmacyMedicationId,
        //                Name = m.Name,
        //                StockOnHand = m.StockOnHand,
        //                ReorderLevel = m.ReorderLevel,
        //                OrderQuantity = 0,
        //                IsSelected = false
        //            }).ToList()
        //    };

        //    var pendingOrders = dbContext.StockOrders
        //        .Include(o => o.PharmacyMedication)
        //        .Where(o => o.Status != "Received")
        //        .ToList();

        //    var ordersSelectList = pendingOrders.Select(o => new
        //    {
        //        o.StockOrderID,
        //        DisplayName = $"{o.PharmacyMedication.Name} - Ordered: {o.OrderQuantity} on {o.Date.ToShortDateString()}"
        //    });

        //    ViewBag.Orders = new SelectList(ordersSelectList, "StockOrderID"/*, "DisplayName"*/);

        //    var combinedModel = new CombinedStockViewModel
        //    {
        //        StockOrder = stockOrderModel,
        //        StockReceived = new StockReceivedViewModel()
        //    };

        //    return View(combinedModel);
        //}
        // GET: ReceivedStock/Create
        public IActionResult StockReceived()
        {
            // Load the orders that are available for selection (you can modify this query as needed)
            var orders = dbContext.StockOrders
                .Include(o => o.PharmacyMedication)
                .Where(o => o.Status != "Received") // You can filter for pending orders if needed
                .ToList();

            // Pass the orders to the view using ViewBag for a dropdown selection
            ViewBag.Orders = new SelectList(orders, "StockOrderID", "PharmacyMedication.Name");

            return View();
        }

        [HttpPost]
        public IActionResult StockReceived(StockReceivedViewModel model)
        {

            // Find the selected stock order from the database
            var stockOrder = dbContext.StockOrders
                .Include(so => so.PharmacyMedication)
                .FirstOrDefault(so => so.StockOrderID == model.StockOrderID);

            if (stockOrder == null)
            {
                return NotFound();
            }

            // Create new StockReceived entry
            var stockReceived = new StockReceived
            {
                StockOrderID = model.StockOrderID,
                PharmacyMedicationId = stockOrder.PharmacyMedicationId,
                ReceivedQuantity = model.ReceivedQuantity,
                ReceivedDate = DateTime.Now
            };

            // Add the received stock entry
            dbContext.StockReceived.Add(stockReceived);

            // Update the StockOnHand in PharmacyMedication
            stockOrder.PharmacyMedication.StockOnHand += model.ReceivedQuantity;

            // Update the status of StockOrder to "Received"
            stockOrder.Status = "Received";

            // Save changes to the database
            dbContext.SaveChanges();

            // return RedirectToAction("ManageStock", "Pharmacist");

            return View();
            // If validation fails, return the form with the entered data
            //return View(model);
        }
        [HttpGet]
        public IActionResult ManageStock(int page = 1, int pageSize = 10)
        {
            var allMedications = dbContext.PharmacyMedication
                .Where(m => m.Name != null && m.StockOnHand != null && m.ReorderLevel != null);

            var totalItems = allMedications.Count();
            var medications = allMedications
                .OrderBy(m => m.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new PharmacyMedicationViewModel
                {
                    PharmacyMedicationId = m.PharmacyMedicationId,
                    Name = m.Name,
                    StockOnHand = m.StockOnHand,
                    ReorderLevel = m.ReorderLevel,
                    OrderQuantity = 0,
                    IsSelected = false
                })
                .ToList();

            var stockOrderModel = new StockViewModel
            {
                Medications = medications
            };

            var pendingOrders = dbContext.StockOrders
                .Include(o => o.PharmacyMedication)
                .Where(o => o.Status != "Received")
                .ToList();

            var ordersSelectList = pendingOrders.Select(o => new
            {
                o.StockOrderID,
                DisplayName = $"{o.PharmacyMedication.Name} - Ordered: {o.OrderQuantity} on {o.Date.ToShortDateString()}"
            });

            ViewBag.Orders = new SelectList(ordersSelectList, "StockOrderID", "DisplayName");

            var combinedModel = new CombinedStockViewModel
            {
                StockOrder = stockOrderModel,
                StockReceived = new StockReceivedViewModel(),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize)
            };

            return View(combinedModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ManageStock(CombinedStockViewModel model)
        {
            // Reload dropdown for the ViewBag (needed even without ModelState)
            var pendingOrders = dbContext.StockOrders
                .Include(o => o.PharmacyMedication)
                .Where(o => o.Status != "Received")
                .ToList();

            var ordersSelectList = pendingOrders.Select(o => new
            {
                o.StockOrderID,
                DisplayName = $"{o.PharmacyMedication.Name} - Ordered: {o.OrderQuantity} on {o.Date.ToShortDateString()}"
            });

            ViewBag.Orders = new SelectList(ordersSelectList, "StockOrderID", "DisplayName");

            var received = model.StockReceived;

            // Manually check if stock order exists
            var stockOrder = dbContext.StockOrders
                .Include(o => o.PharmacyMedication)
                .FirstOrDefault(o => o.StockOrderID == received.StockOrderID);

            if (stockOrder == null)
            {
                TempData["Error"] = "Invalid stock order.";
                return View(model);
            }

            var medication = dbContext.PharmacyMedication
                .FirstOrDefault(m => m.PharmacyMedicationId == stockOrder.PharmacyMedicationId);

            if (medication == null)
            {
                TempData["Error"] = "Medication not found.";
                return View(model);
            }

            // Update StockOnHand
            medication.StockOnHand += received.ReceivedQuantity;

            // Update order status
            stockOrder.Status = "Received";

            // Create and add received stock record
            var receivedStock = new StockReceived
            {
                ReceivedQuantity = received.ReceivedQuantity,
                ReceivedDate = DateTime.Now,
                StockOrderID = stockOrder.StockOrderID,
                PharmacyMedicationId = medication.PharmacyMedicationId
            };

            dbContext.StockReceived.Add(receivedStock);
            dbContext.SaveChanges();

            TempData["Success"] = "Stock received successfully.";
            return RedirectToAction("ManageStock");
        }


        //[HttpGet]
        //public IActionResult ManageStock(int page = 1, int pageSize = 10)
        //{
        //    var allMedications = dbContext.PharmacyMedication
        //        .Where(m => m.Name != null && m.StockOnHand != null && m.ReorderLevel != null);

        //    var totalItems = allMedications.Count();
        //    var medications = allMedications
        //        .OrderBy(m => m.Name)
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .Select(m => new PharmacyMedicationViewModel
        //        {
        //            PharmacyMedicationId = m.PharmacyMedicationId,
        //            Name = m.Name,
        //            StockOnHand = m.StockOnHand,
        //            ReorderLevel = m.ReorderLevel,
        //            OrderQuantity = 0,
        //            IsSelected = false
        //        })
        //        .ToList();

        //    var stockOrderModel = new StockViewModel
        //    {
        //        Medications = medications
        //    };

        //    var pendingOrders = dbContext.StockOrders
        //        .Include(o => o.PharmacyMedication)
        //        .Where(o => o.Status != "Received")
        //        .ToList();

        //    var ordersSelectList = pendingOrders.Select(o => new
        //    {
        //        o.StockOrderID,
        //        DisplayName = $"{o.PharmacyMedication.Name} - Ordered: {o.OrderQuantity} on {o.Date.ToShortDateString()}"
        //    });

        //    ViewBag.Orders = new SelectList(ordersSelectList, "StockOrderID", "DisplayName");

        //    var combinedModel = new CombinedStockViewModel
        //    {
        //        StockOrder = stockOrderModel,
        //        StockReceived = new StockReceivedViewModel(),
        //        CurrentPage = page,
        //        TotalPages = (int)Math.Ceiling((double)totalItems / pageSize)
        //    };

        //    var model = new CombinedStockViewModel
        //    {
        //        Medications = dbContext.PharmacyMedication.ToList(),
        //        StockOrders = dbContext.StockOrders.ToList(),
        //        //ReceivedStocks = dbContext.StockReceived.ToList()
        //    };

        //    return View(combinedModel);
        //}


        // POST: Handle stock ordering
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceStockOrder(CombinedStockViewModel model)
        {
            var emailService = new EmailService(_configuration);
            var purchaseManagerEmail = "qilinganakamva@gmail.com";
            var orderedMedications = new List<string>();

            var selectedMeds = model.StockOrder?.Medications?.Where(m => m.IsSelected && m.OrderQuantity > 0).ToList();

            if (selectedMeds == null || !selectedMeds.Any())
            {
                TempData["ErrorMessage"] = "No medications selected for order.";
                return RedirectToAction("ManageStock");
            }

            foreach (var item in selectedMeds)
            {
                var medication = await dbContext.PharmacyMedication.FindAsync(item.PharmacyMedicationId);
                if (medication == null) continue;

                var newOrder = new StockOrder
                {
                    PharmacyMedicationId = item.PharmacyMedicationId,
                    OrderQuantity = item.OrderQuantity,
                    Date = DateTime.Now,
                    Status = "Ordered"
                };

                dbContext.StockOrders.Add(newOrder);
                orderedMedications.Add($"{medication.Name} - Quantity: {item.OrderQuantity}");
            }

            await dbContext.SaveChangesAsync();

            if (orderedMedications.Any())
            {
                var subject = "Stock Order Notification";
                var message = $"Dear Purchase Manager,<br/><br/>The following medications have been ordered:<br/>" +
                              string.Join("<br/>", orderedMedications) +
                              "<br/><br/>Best regards,<br/>Pharmacy Team<br/></br/><small>Bay Breeze Day Hospital<br/>Eastern Cape<br/>Summerstrand (6001)<br/>1 8th Avenue<br/>041 58 2121</small>";

                await emailService.SendEmailAsync(purchaseManagerEmail, subject, message);
            }

            TempData["SuccessMessage"] = "Stock order placed and notification sent.";
            return RedirectToAction("ManageStock");
        }

        //public IActionResult DispenseMedication(int prescriptionId)
        //{
        //    var prescription = dbContext.NewPrescriptions
        //        .Include(p => p.PrescriptionMedications)
        //            .ThenInclude(pm => pm.PharmacyMedication)
        //                .ThenInclude(m => m.MedicationActiveIngredients)
        //        .Include(p => p.PatientProfile)
        //            .ThenInclude(p => p.PatientAllergies)
        //        .FirstOrDefault(p => p.PrescriptionId == prescriptionId);

        //    if (prescription == null)
        //        return NotFound();

        //    if (prescription.Status == "Dispensed")
        //    {
        //        TempData["InfoMessage"] = "This prescription has already been dispensed.";
        //        return RedirectToAction("PrescriptionList"); // or your desired view
        //    }

        //    var patientAllergies = prescription.PatientProfile.PatientAllergies
        //        .Select(a => a.ActiveIngredientsID)
        //        .ToList();

        //    var medicationIngredientIds = prescription.PrescriptionMedications
        //        .SelectMany(pm => pm.PharmacyMedication.MedicationActiveIngredients)
        //        .Select(ai => ai.ActiveIngredientsID)
        //        .ToList();

        //    var allergicIngredients = medicationIngredientIds.Intersect(patientAllergies).ToList();

        //    var patientConditions = dbContext.PatientConditions
        //        .Where(pc => pc.PatientProfileId == prescription.PatientProfileId)
        //        .Select(pc => pc.ConditionID)
        //        .ToList();

        //    var contraindicatedIngredients = dbContext.ContraIndications
        //        .Where(ci => patientConditions.Contains(ci.ConditionID))
        //        .Select(ci => ci.ActiveIngredientsID)
        //        .ToList();

        //    var contraindications = medicationIngredientIds.Intersect(contraindicatedIngredients).ToList();

        //    if (allergicIngredients.Any() || contraindications.Any())
        //    {
        //        var warningMessage = "⚠️ Warning! This medication may be harmful due to: ";
        //        if (allergicIngredients.Any())
        //            warningMessage += "patient allergy. ";
        //        if (contraindications.Any())
        //            warningMessage += "existing condition contraindication.";

        //        TempData["WarningMessage"] = warningMessage;
        //        TempData["PrescriptionId"] = prescriptionId;

        //        return RedirectToAction("ConfirmDispensePopup");
        //    }

        //return RedirectToAction("ManagePrescription", new { prescriptionId });
        // return RedirectToAction("PrescriptionList");
        //    return RedirectToAction("ManagePrescription", "Surgeon", new { prescriptionId });

        // }
        public IActionResult DispenseMedication(int prescriptionId)
        {
            var prescription = dbContext.NewPrescriptions
                .Include(p => p.PrescriptionMedications)
                    .ThenInclude(pm => pm.PharmacyMedication)
                        .ThenInclude(m => m.MedicationActiveIngredients)
                .Include(p => p.PatientProfile)
                    .ThenInclude(p => p.PatientAllergies)
                .FirstOrDefault(p => p.PrescriptionId == prescriptionId);

            if (prescription == null)
                return NotFound();

            if (prescription.Status == "Dispensed")
            {
                TempData["InfoMessage"] = "This prescription has already been dispensed.";
                return RedirectToAction("PrescriptionList");
            }

            // Check for allergies
            var patientAllergies = prescription.PatientProfile.PatientAllergies
                .Select(a => a.ActiveIngredientsID)
                .ToList();

            var medicationIngredientIds = prescription.PrescriptionMedications
                .SelectMany(pm => pm.PharmacyMedication.MedicationActiveIngredients)
                .Select(ai => ai.ActiveIngredientsID)
                .ToList();

            var allergicIngredients = medicationIngredientIds.Intersect(patientAllergies).ToList();

            // Check for contraindications
            var patientConditions = dbContext.PatientConditions
                .Where(pc => pc.PatientProfileId == prescription.PatientProfileId)
                .Select(pc => pc.ConditionID)
                .ToList();

            var contraindicatedIngredients = dbContext.ContraIndications
                .Where(ci => patientConditions.Contains(ci.ConditionID))
                .Select(ci => ci.ActiveIngredientsID)
                .ToList();

            var contraindications = medicationIngredientIds.Intersect(contraindicatedIngredients).ToList();

            // If allergy or contraindication exists, redirect to warning
            if (allergicIngredients.Any() || contraindications.Any())
            {
                var warningMessage = "⚠️ Warning! This medication may be harmful due to: ";
                if (allergicIngredients.Any())
                    warningMessage += "patient allergy. ";
                if (contraindications.Any())
                    warningMessage += "existing condition contraindication.";

                TempData["WarningMessage"] = warningMessage;
                TempData["PrescriptionId"] = prescriptionId;

                return RedirectToAction("ConfirmDispensePopup");
            }

            // Dispense logic: Check and deduct stock
            foreach (var pm in prescription.PrescriptionMedications)
            {
                var med = pm.PharmacyMedication;
                if (med.StockOnHand >= pm.Quantity)
                {
                    med.StockOnHand -= pm.Quantity;
                }
                else
                {
                    TempData["ErrorMessage"] = $"Not enough stock for {med.Name}.";
                    return RedirectToAction("PrescriptionList");
                }
            }

            // Update prescription status
            prescription.Status = "Dispensed";
            dbContext.SaveChanges();

            TempData["SuccessMessage"] = "Prescription dispensed successfully.";

            // Redirect to another controller, e.g. Surgeon
            return RedirectToAction("PrescriptionList", "Surgeon", new { prescriptionId });
        }


        public IActionResult ConfirmDispensePopup()
        {
            ViewBag.WarningMessage = TempData["WarningMessage"];
            ViewBag.PrescriptionId = TempData["PrescriptionId"];
            return View();
        }

        [HttpPost]
        public IActionResult CompleteDispense(int prescriptionId)
        {
            var prescription = dbContext.NewPrescriptions
                .Include(p => p.PrescriptionMedications)
                    .ThenInclude(pm => pm.PharmacyMedication)
                .FirstOrDefault(p => p.PrescriptionId == prescriptionId);

            if (prescription == null)
                return NotFound();

            if (prescription.Status == "Dispensed")
            {
                TempData["InfoMessage"] = "Prescription already dispensed.";
                return RedirectToAction("PrescriptionList");
            }

            // Dispense logic: Decrease stock
            foreach (var pm in prescription.PrescriptionMedications)
            {
                var med = pm.PharmacyMedication;

                if (med.StockOnHand >= pm.Quantity)
                {
                    med.StockOnHand -= pm.Quantity;
                }
                else
                {
                    TempData["ErrorMessage"] = $"Not enough stock for {med.Name}.";
                    return RedirectToAction("PrescriptionList");
                }
            }

            prescription.Status = "Dispensed";
            dbContext.SaveChanges();

            TempData["SuccessMessage"] = "Prescription dispensed successfully.";
            return RedirectToAction("PrescriptionList");
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

        



    }





}
