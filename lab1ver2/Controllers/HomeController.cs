using lab1ver2.Models;
using lab1ver2.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace lab1ver2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;

        public HomeController(ILogger<HomeController> logger, IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contact(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            await _emailSender.SendEmailAsync(contact.Name, contact.Email, contact.Subject, contact.Message);
            _logger.LogInformation($"Contact email:{contact.Email}; [Message]{contact.Message}");

            return View();
        }

        public IActionResult Post()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}