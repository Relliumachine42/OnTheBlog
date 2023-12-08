using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnTheBlog.Data;
using OnTheBlog.Models;
using System.Diagnostics;


namespace OnTheBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<BlogUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailService;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, UserManager<BlogUser> userManager, ApplicationDbContext context, IEmailSender emailService, IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _emailService = emailService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> ContactMe()
        {
            string? blogUserId = _userManager.GetUserId(User);

            if (blogUserId == null)
            {
                return NotFound();
            }

            BlogUser? blogUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == blogUserId);

            return View(blogUser);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactMe([Bind("FirstName,LastName,Email")] BlogUser blogUser, string? message)
        {
            string? swalMessage = string.Empty;
           
            if (ModelState.IsValid)
            {
                try
                {
                    string? contactEmail = _configuration["ContactMeEmail"] ?? Environment.GetEnvironmentVariable("ContactMeEmail");
                    await _emailService.SendEmailAsync(contactEmail!, $"InTheBlogLight Contact - {blogUser.FullName} - {blogUser.Email}", message!);
                    swalMessage = "Email sent successfully!";
                }
                catch (Exception)
                {

                    throw;
                }


            } else
            {

                swalMessage = "Error: Unable to send email.";
            }
            return RedirectToAction("ContactMe", new { swalMessage });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}