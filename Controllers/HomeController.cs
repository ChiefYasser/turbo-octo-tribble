using HRRecruitmentSystem.Data;
using HRRecruitmentSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HRRecruitmentSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
          
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);

                // Count Active Jobs
                ViewBag.ActiveJobs = await _context.JobOffers.CountAsync(j => j.IsActive);

                if (User.IsInRole("Admin") || User.IsInRole("HR"))
                {
                    // Admin Stats
                    ViewBag.PendingApps = await _context.Applications.CountAsync(a => a.Status == ApplicationStatus.Pending);
                }
                else
                {
                    // Candidate Stats
                    var candidate = await _context.Candidates.FirstOrDefaultAsync(c => c.UserId == user.Id);
                    if (candidate != null)
                    {
                        ViewBag.MyAppsCount = await _context.Applications.CountAsync(a => a.CandidateId == candidate.Id);
                        ViewBag.MyAccepted = await _context.Applications.CountAsync(a => a.CandidateId == candidate.Id && a.Status == ApplicationStatus.Accepted);
                    }
                    else
                    {
                        ViewBag.MyAppsCount = 0;
                        ViewBag.MyAccepted = 0;
                    }
                }
            }
            return View();
        }

        public IActionResult Privacy()
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