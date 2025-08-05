using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

using Microsoft.AspNetCore.Http;

using System.Security.Claims;
using E_PRESCRIBING_SYSTEM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using E_PRESCRIBING_SYSTEM.Data;
//using E_PRESCRIBING_SYSTEM.EmailerSender;


namespace E_PRESCRIBING_SYSTEM.Controllers
{
    public class EmailController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IConfiguration _configuration;

        public EmailController(ApplicationDbContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index(E_PRESCRIBING_SYSTEM.Models.Email model)
        {
            MailMessage mail = new MailMessage(model.from, model.to);
            mail.Subject = model.subject;
            mail.Body = model.body;
            mail.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();
             smtp.Host = "smtp.gmail.com";
            smtp.Port = 570;
            smtp.EnableSsl = true;



            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StockOrder/*OrderMedications*/(StockViewModel model)
        {
            var emailService = new EmailService(_configuration);
            var purchaseManagerEmail = "qilinganakamva@gmail.com"; /*"zisandanodali01@gmail.com";*/ // Replace with actual email

            var orderedMedications = new List<string>(); // To track medications being ordered

            foreach (var item in model.Medications.Where(m => m.IsSelected && m.OrderQuantity > 0))
            {
                // Fetch the medication name if not already in the view model
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

                // Use the fetched medication name (or ensure it's in the model)
                orderedMedications.Add($"{pharmacyMedication.Name} - Quantity: {item.OrderQuantity}");
            }

            await dbContext.SaveChangesAsync();

            // If medications were ordered, send an email to the purchase manager
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

        //[HttpGet]
        //public IActionResult SendEmail()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult SendEmail(string toEmail, string subject, string body, IFormFile attachment)
        //{
        //    // Get the logged-in user's email
        //    string fromEmail = User.FindFirstValue(ClaimTypes.Email);

        //    // Send email using the EmailHelper
        //    EmailHelper.SendEmail(fromEmail, toEmail, subject, body, attachment);

        //    return RedirectToAction("SendEmail"); // Redirect to a confirmation page
        //}

        public ActionResult SendEmail()
        {
            try
            {
                // Create a MailMessage object
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("your_email@gmail.com");
                mailMessage.To.Add("recipient_email@example.com");
                mailMessage.Subject = "Subject of your email";
                mailMessage.Body = "Body content of your email";

                // Configure the SmtpClient
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new System.Net.NetworkCredential("your_email@gmail.com", "your_password");

                // Send the email
                smtpClient.Send(mailMessage);

                // Return success message
                return Content("Email sent successfully!");
            }
            catch (SmtpException ex)
            {
                // Handle any errors
                return Content($"Error sending email: {ex.Message}");
            }
        }




    }
}
