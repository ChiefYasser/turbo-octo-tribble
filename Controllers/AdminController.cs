using HRRecruitmentSystem.Data;
using HRRecruitmentSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRRecruitmentSystem.Controllers
{
    [Authorize(Roles = "Admin,HR")] // Only for HR and Admin
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ GET: Dashboard with Charts
        public async Task<IActionResult> Dashboard()
        {
            // 1. Data for Pie Chart: Applications per Job
            var jobStats = await _context.Applications
                .Include(a => a.JobOffer)
                .GroupBy(a => a.JobOffer.Title)
                .Select(g => new { JobTitle = g.Key, Count = g.Count() })
                .ToListAsync();

            // 2. Data for Bar Chart: Status counts
            var statusStats = await _context.Applications
                .GroupBy(a => a.Status)
                .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                .ToListAsync();

            // Pass Arrays to View
            ViewBag.JobLabels = jobStats.Select(j => j.JobTitle).ToArray();
            ViewBag.JobData = jobStats.Select(j => j.Count).ToArray();

            ViewBag.StatusLabels = statusStats.Select(s => s.Status).ToArray();
            ViewBag.StatusData = statusStats.Select(s => s.Count).ToArray();

            // Card Counters
            ViewBag.TotalJobs = await _context.JobOffers.CountAsync();
            ViewBag.TotalApplications = await _context.Applications.CountAsync();

            return View();
        }

        // GET: List all applications (Filterable)
        public async Task<IActionResult> Applications(int? id)
        {
            var query = _context.Applications
                .Include(a => a.Candidate)
                .Include(a => a.JobOffer)
                .AsQueryable();

            // If a specific Job ID is provided, filter by it
            if (id.HasValue)
            {
                query = query.Where(a => a.JobOfferId == id);
                ViewBag.JobTitle = _context.JobOffers.Find(id)?.Title;
            }
            else
            {
                ViewBag.JobTitle = "All Jobs";
            }

            return View(await query.OrderByDescending(a => a.AppliedAt).ToListAsync());
        }

        // POST: Change Status (Approved/Rejected)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, ApplicationStatus status)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application != null)
            {
                application.Status = status;
                await _context.SaveChangesAsync();
            }
            // Return to the previous page
            return RedirectToAction(nameof(Applications));
        }
    }
}