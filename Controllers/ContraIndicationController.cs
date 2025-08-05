using E_PRESCRIBING_SYSTEM.Data;
using E_PRESCRIBING_SYSTEM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace E_PRESCRIBING_SYSTEM.Controllers
{
    public class ContraIndicationController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public ContraIndicationController(ApplicationDbContext dBD)
        {
            dbContext = dBD;
        }


        // GET: ContraIndication/Create
        //public IActionResult Create()
        //{
        //    ViewBag.ActiveIngredients = dbContext.ActiveIngredients.ToList();
        //    ViewBag.Conditions = dbContext.conditions.ToList();
        //    return View();
        //}
        //public IActionResult Create()
        //{
        //    ViewBag.ActiveIngredients = dbContext.ActiveIngredients.ToList();
        //     ViewBag.Conditions = dbContext.conditions.ToList();
        //    return View();

        //}

        //// POST: ContraIndication/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(ContraIndication contraIndication)
        //{

        //        dbContext.ContraIndications.Add(contraIndication);
        //        dbContext.SaveChanges();
        //        return RedirectToAction("Index", "ContraIndication");

        //     //ViewBag.Medications = dbContext.PharmacyMedication.ToList();
        //    //ViewBag.Conditions = dbContext.conditions.ToList();
        //    //return View(contraIndication);
        //}

        // GET: ContraIndication/Index
        public IActionResult Index()
        {
            var contraindications = dbContext.ContraIndications
                .Include(c => c.ActiveIngredients)
                .Include(c => c.Condition)
                .ToList();
            return View(contraindications);
        }

        // GET: ContraIndication/Create
        public IActionResult Create()
        {
            var viewModel = new ContraIndicationViewModel
            {
                ActiveIngredients = dbContext.ActiveIngredients.ToList(),
                Conditions = dbContext.conditions.ToList()
            };
            return View(viewModel);
        }

        // POST: ContraIndication/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ContraIndicationViewModel viewModel)
        {
          
                dbContext.ContraIndications.Add(viewModel.ContraIndication);
                dbContext.SaveChanges();
             
            

            // If the model is invalid, reload ActiveIngredients and Conditions
            viewModel.ActiveIngredients = dbContext.ActiveIngredients.ToList();
            viewModel.Conditions = dbContext.conditions.ToList();
            return RedirectToAction("Index");
        }



    }

}

