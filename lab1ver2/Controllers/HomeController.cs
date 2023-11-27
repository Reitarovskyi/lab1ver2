using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using lab1ver2.Data;
using lab1ver2.Models;
using lab1ver2.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace lab1ver2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private readonly IValidator<Contact> _validator;

        public HomeController(ILogger<HomeController> logger, IEmailSender emailSender, ApplicationDbContext context, IValidator<Contact> validator)
        {
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
            _validator = validator;
        }

        public IActionResult Index()
        {
            return View(_context.Posts.ToList());
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
            ValidationResult result = _validator.Validate(contact);
            
            if (!result.IsValid)
            {
                result.AddToModelState(ModelState);
                return View(contact);
            }

            await _emailSender.SendEmailAsync(contact.Name, contact.Email, contact.Subject, contact.Message);
            _logger.LogInformation($"Contact email:{contact.Email}; [Message]{contact.Message}");

            return View();
        }
        public IActionResult Post()
        {
            return View();
        }
        public IActionResult ReadPost(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id);
            return View(post);
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}