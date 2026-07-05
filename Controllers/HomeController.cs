using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using AerodyneCompressors.Models;
using Microsoft.AspNetCore.Mvc;

namespace AerodyneCompressors.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration; //  Config data read karne ke liye injection variables

        // Constructor update kiya settings properties read karne ke liye
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        // GET: /Home/ContactUs
        public IActionResult ContactUs()
        {
            return View();
        }

        // 🚀 THE CORE BE BACKEND ENGINE: Processing Form Data securely via SMTP
        [HttpPost]
        [ValidateAntiForgeryToken] // Enforces secure validation layers blocking cross-site attacks
        public IActionResult ContactUs(ContactFormModel model)
        {
            // Frontend validation checks trace logs bypass validation matrix
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // AppSettings files ke andar se SMTP secure nodes capture arrays read karna
                var smtpServer = _configuration["SmtpSettings:Server"];
                var smtpPort = int.Parse(_configuration["SmtpSettings:Port"] ?? "587");
                var senderName = _configuration["SmtpSettings:SenderName"];
                var senderEmail = _configuration["SmtpSettings:SenderEmail"];
                var receiverEmail = _configuration["SmtpSettings:ReceiverEmail"];
                var appPassword = _configuration["SmtpSettings:Password"];

                // Elegant Corporate Layout me Mail content structure design template parameters setup karna
                string mailSubject = $"AERODYNE Portal Inquiry: {model.Subject}";
                string mailBody = $@"
                    <div style='font-family: Arial, sans-serif; border: 1px solid #004aad; border-radius: 8px; padding: 25px; max-width: 600px; background-color: #f8f9fa;'>
                        <h2 style='color: #004aad; border-bottom: 2px solid #004aad; padding-bottom: 10px; margin-top: 0;'>New Business Inquiry</h2>
                        <p style='font-size: 14px;'><strong>Full Name:</strong> {model.Name}</p>
                        <p style='font-size: 14px;'><strong>Email Address:</strong> <a href='mailto:{model.Email}'>{model.Email}</a></p>
                        <p style='font-size: 14px;'><strong>Phone Number:</strong> {model.Phone}</p>
                        <p style='font-size: 14px;'><strong>Company Name:</strong> {(string.IsNullOrEmpty(model.Company) ? "Not Provided" : model.Company)}</p>
                        <p style='font-size: 14px;'><strong>Inquiry Subject:</strong> {model.Subject}</p>
                        <div style='margin-top: 20px; padding: 15px; background-color: #ffffff; border-left: 4px solid #00d4ff; border-radius: 4px;'>
                            <p style='font-size: 14px; margin-top: 0; font-weight: bold; color: #333;'>Question / Message Details:</p>
                            <p style='font-size: 14px; color: #555; line-height: 1.6; white-space: pre-wrap;'>{model.Question}</p>
                        </div>
                        <hr style='border: 0; border-top: 1px solid #eee; margin-top: 25px;' />
                        <p style='font-size: 11px; color: #999; text-align: center; margin-bottom: 0;'>This inquiry was securely dispatched directly from the official AERODYNE Compressors layout portal hub.</p>
                    </div>";

                // .NET Mail Message Framework Object parameters assign array mapping
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(senderEmail, senderName);
                    message.To.Add(new MailAddress(receiverEmail));
                    message.Subject = mailSubject;
                    message.Body = mailBody;
                    message.IsBodyHtml = true; // Enforce HTML formatting system visibility triggers
                    message.ReplyToList.Add(new MailAddress(model.Email, model.Name)); // Directly hitting reply matches sender client

                    // SMTP Network Framework initialization logs gateway
                    using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
                    {
                        smtpClient.Credentials = new NetworkCredential(senderEmail, appPassword);
                        smtpClient.EnableSsl = true; // Secured cryptographic SSL handshake allocation protocol Enforced

                        // Triggers the real physical mail transfer event right now!
                        smtpClient.Send(message);
                    }
                }

                // TempData stores safe alert indicators variables across pages reloads lifecycle
                TempData["SuccessMessage"] = "Thank you! Your message has been sent successfully. Our team will contact you shortly.";

                // Pure Post-Redirect-Get pattern strategy to clean parameters bounds
                return RedirectToAction(nameof(ContactUs));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to compile/dispatch industrial inquiry web mail through portal system hubs.");
                ModelState.AddModelError(string.Empty, "An unexpected server error occurred while sending your message. Please try calling directly.");
                return View(model);
            }
        }

        public IActionResult Product()
        {
            return View();
        }

        public IActionResult OEMSolution()
        {
            return View();
        }

        // GET: /Home/Calculation
        [HttpGet]
        public IActionResult Calculation()
        {
            return View();
        }

        // POST: /Home/Calculation
        [HttpPost]
        public IActionResult Calculation(string clientEmail, string clientPhone, string motorHp, string electricityRate, string runningHours, string demandProfile, string techStatus)
        {
            if (string.IsNullOrEmpty(clientEmail) || string.IsNullOrEmpty(clientPhone))
            {
                TempData["ErrorMessage"] = "Please provide valid corporate coordinates.";
                return RedirectToAction("Calculation");
            }

            try
            {
                // 🚀 ADVANCED MATHEMATICAL PARSING CORE (45% FACTOR COMPLIANCE)
                double hp = double.TryParse(motorHp, out var rHp) ? rHp : 30;
                double rate = double.TryParse(electricityRate, out var rRate) ? rRate : 8;
                double hours = double.TryParse(runningHours, out var rHours) ? rHours : 4000;

                double kw = hp * 0.746;
                double savingFactor = 0.05; // Base fallback

                if (demandProfile == "fluctuating" && techStatus == "fixed")
                {
                    savingFactor = 0.45; // 🎯 FIX 4: Recalculated precisely based on 45% benchmark metrics
                }
                else if (demandProfile == "stable" && techStatus == "fixed")
                {
                    savingFactor = 0.15;
                }

                double annualSavings = kw * hours * rate * savingFactor;
                string finalSavingsFormatted = "₹ " + annualSavings.ToString("N0", new System.Globalization.CultureInfo("en-IN"));

                // Fetching secure credentials from configuration model sheets
                var smtpServer = _configuration["SmtpSettings:Server"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(_configuration["SmtpSettings:Port"] ?? "587");
                var senderEmail = _configuration["SmtpSettings:SenderEmail"] ?? "aerodyne.equipments@gmail.com";
                var senderName = _configuration["SmtpSettings:SenderName"] ?? "AERODYNE EQUIPMENTS";
                var senderPass = _configuration["SmtpSettings:Password"] ?? "";

                // Construction of corporate intelligence dispatch data
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(senderEmail, senderName);
                mailMessage.To.Add("aerodyne.equipments@gmail.com"); // Notifying your head office yard
                mailMessage.Subject = $"NEW AUDIT LEAD: Savings Generated for {clientPhone}";
                mailMessage.IsBodyHtml = true;

                mailMessage.Body = $@"
                    <div style='font-family: Arial, sans-serif; background: #0f172a; color: #ffffff; padding: 30px; border-radius: 12px;'>
                        <h2 style='color: #00d4ff; border-bottom: 2px solid #004aad; padding-bottom: 10px;'>AERODYNE SYSTEM CONTEXT AUDIT INTERMAP</h2>
                        <p style='font-size: 1.1rem;'>A new potential factory partner has generated an audit calculation sheet:</p>
                        <table style='width: 100%; border-collapse: collapse; margin-top: 20px; font-size: 0.95rem;'>
                            <tr><td style='padding: 10px; border: 1px solid #1e293b; background: #0b1224; font-weight: bold;'>Corporate Email:</td><td style='padding: 10px; border: 1px solid #1e293b;'>{clientEmail}</td></tr>
                            <tr><td style='padding: 10px; border: 1px solid #1e293b; background: #0b1224; font-weight: bold;'>Mobile / Contact:</td><td style='padding: 10px; border: 1px solid #1e293b;'>{clientPhone}</td></tr>
                            <tr><td style='padding: 10px; border: 1px solid #1e293b; background: #0b1224; font-weight: bold;'>Motor Selection:</td><td style='padding: 10px; border: 1px solid #1e293b;'>{hp} HP</td></tr>
                            <tr><td style='padding: 10px; border: 1px solid #1e293b; background: #0b1224; font-weight: bold;'>Grid Cost Unit:</td><td style='padding: 10px; border: 1px solid #1e293b;'>₹ {rate} / kWh</td></tr>
                            <tr><td style='padding: 10px; border: 1px solid #1e293b; background: #0b1224; font-weight: bold;'>Operational Duty:</td><td style='padding: 10px; border: 1px solid #1e293b;'>{hours} Hours / Year</td></tr>
                            <tr><td style='padding: 10px; border: 1px solid #1e293b; background: #0b1224; font-weight: bold; color: #00d4ff;'>ESTIMATED SAVINGS:</td><td style='padding: 10px; border: 1px solid #1e293b; font-weight: bold; color: #00ff66;'>{finalSavingsFormatted} / Annual</td></tr>
                        </table>
                    </div>";

                // SMTP Client Engine Configurations
                using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPass);
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(mailMessage);
                }

                // Return payload indicators back to the layout pipeline state trackers
                TempData["Message"] = "Verification Successful! Your automated energy summary has been mapped layout.";
                TempData["VerifiedSavings"] = finalSavingsFormatted;
                TempData["SavedHp"] = motorHp;
                TempData["SavedRate"] = electricityRate;
                TempData["SavedHours"] = runningHours;
                TempData["SavedDemand"] = demandProfile;
                TempData["SavedTech"] = techStatus;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SMTP Controller framework crashed during dispatch.");
                TempData["ErrorMessage"] = "Email Server connection timeout. But your calculation is unlocked below!";
                TempData["VerifiedSavings"] = "UNLOCKED";
            }

            return RedirectToAction("Calculation");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

