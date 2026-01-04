using HRRecruitmentSystem.Data;
using HRRecruitmentSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRRecruitmentSystem.Controllers
{
    [Authorize]
    public class ApplicationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public ApplicationController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        // GET: Show the Apply form
        public async Task<IActionResult> Apply(int id)
        {
            var job = await _context.JobOffers.FindAsync(id);
            if (job == null) return NotFound();

            var model = new JobApplicationViewModel
            {
                JobOfferId = job.Id,
                JobTitle = job.Title
            };

            return View(model);
        }

        // POST: Process the application
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(JobApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var candidate = await _context.Candidates.FirstOrDefaultAsync(c => c.UserId == user.Id);

                // Handle File Upload
                string uniqueFileName = null;
                if (model.CvFile != null)
                {
                    if (model.CvFile.ContentType != "application/pdf")
                    {
                        ModelState.AddModelError("CvFile", "Only PDF files are allowed.");
                        return View(model);
                    }
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder);
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.CvFile.FileName;
                    using (var fileStream = new FileStream(Path.Combine(uploadsFolder, uniqueFileName), FileMode.Create))
                    {
                        await model.CvFile.CopyToAsync(fileStream);
                    }
                }

                // Create Candidate if not exists
                if (candidate == null)
                {
                    candidate = new Candidate
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        FirstName = "Candidate",
                        LastName = "User",
                        ResumePath = uniqueFileName
                    };
                    _context.Candidates.Add(candidate);
                    await _context.SaveChangesAsync();
                }
                else if (uniqueFileName != null)
                {
                    candidate.ResumePath = uniqueFileName;
                    _context.Candidates.Update(candidate);
                }

                // Check duplicate
                bool alreadyApplied = await _context.Applications.AnyAsync(a => a.CandidateId == candidate.Id && a.JobOfferId == model.JobOfferId);
                if (alreadyApplied)
                {
                    ModelState.AddModelError("", "You have already applied for this job.");
                    return View(model);
                }

                var application = new Application
                {
                    JobOfferId = model.JobOfferId,
                    CandidateId = candidate.Id,
                    Status = ApplicationStatus.Pending,
                    AppliedAt = DateTime.Now
                };

                _context.Applications.Add(application);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyApplications)); // Redirect to My Page
            }
            return View(model);
        }

        // ✅ NEW: Candidate Dashboard
        public async Task<IActionResult> MyApplications()
        {
            var user = await _userManager.GetUserAsync(User);
            var candidate = await _context.Candidates.FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (candidate == null)
            {
                // User hasn't applied to anything yet
                return View(new List<Application>());
            }

            var myApps = await _context.Applications
                .Include(a => a.JobOffer)
                .Where(a => a.CandidateId == candidate.Id)
                .OrderByDescending(a => a.AppliedAt)
                .ToListAsync();

            return View(myApps);
        }
    }
}