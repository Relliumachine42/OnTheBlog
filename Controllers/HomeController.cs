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
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailService;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, UserManager<BlogUser> userManager, ApplicationDbContext context, IConfiguration configuration, IEmailSender emailService, IUserService userService)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> ContactMe(string? swalMessage = null)
        {
            ViewData["SwalMessage"] = swalMessage;

            string? blogUserId = _userManager.GetUserId(User);

            BlogUser? blogUser = await _userService.GetUserByIdAsync(blogUserId)
                ?? new BlogUser();

            return View(blogUser);
        }

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
                    return RedirectToAction("Index", "BlogPosts", new { swalMessage });
                }
                catch (Exception)
                {
                    swalMessage = "Error: Unable to send email.";
                    return RedirectToAction(nameof(ContactMe), new { swalMessage });
                }


            } else
            {

                swalMessage = "Error: All fields are required.";
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