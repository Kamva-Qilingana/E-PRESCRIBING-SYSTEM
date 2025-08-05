using E_PRESCRIBING_SYSTEM.Data;
using E_PRESCRIBING_SYSTEM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;

public class ReportController : Controller
{
    private readonly ApplicationDbContext dbContext;

    public ReportController(ApplicationDbContext context)
    {
        dbContext = context;
    }

    // GET: Show date selection form
    public IActionResult Generate()
    {
        var viewModel = new PharmacistReportViewModel
        {
            StartDate = DateTime.Now.AddMonths(-1),
            EndDate = DateTime.Now,
            ReportGeneratedDate = DateTime.Now
        };
        return View(viewModel);
    }

    // POST: Generate report based on date range
    [HttpPost]
    public IActionResult GenerateReport(DateTime startDate, DateTime endDate)
    {
        // Get prescriptions within date range
        var prescriptions = dbContext.NewPrescriptions
            .Include(p => p.PatientProfile)
            .Include(p => p.PrescriptionMedications)
                .ThenInclude(pm => pm.PharmacyMedication)
            .Where(p => p.DateAdded >= startDate && p.DateAdded <= endDate)
            .OrderBy(p => p.DateAdded)
            .ToList();

        // Count dispensed and rejected
        var totalDispensed = prescriptions.Count(p => p.Status == "Dispensed");
        var totalRejected = prescriptions.Count(p => p.Status == "Rejected");

        // Get all dispensed prescription medications
        var dispensedMedicationItems = dbContext.PrescriptionMedications
            .Include(pm => pm.PharmacyMedication)
            .Include(pm => pm.NewPrescription)
            .Where(pm => pm.NewPrescription.Status == "Dispensed"
                      && pm.NewPrescription.DateAdded >= startDate
                      && pm.NewPrescription.DateAdded <= endDate)
            .ToList();

        // Summarize by medication name
        var medicationSummary = dispensedMedicationItems
            .GroupBy(pm => pm.PharmacyMedication.Name)
            .ToDictionary(g => g.Key, g => g.Sum(pm => pm.Quantity));

        var viewModel = new PharmacistReportViewModel
        {
            StartDate = startDate,
            EndDate = endDate,
            ReportGeneratedDate = DateTime.Now,
            Prescriptions = prescriptions,
            TotalScriptsDispensed = totalDispensed,
            TotalScriptsRejected = totalRejected,
            MedicationSummary = medicationSummary
        };

        return View("PharmacistReport", viewModel);
    }
}
