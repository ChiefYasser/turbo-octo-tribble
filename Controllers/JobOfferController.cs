using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRRecruitmentSystem.Data;
using HRRecruitmentSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace HRRecruitmentSystem.Controllers
{
   
    [Authorize(Roles = "Admin,HR")]
    public class JobOfferController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobOfferController(ApplicationDbContext context)
        {
            _context = context;
        }

      
        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchString)
        {
            var jobs = from j in _context.JobOffers
                       select j;

            if (!String.IsNullOrEmpty(searchString))
            {
                // Filter logic: Check Title OR Description OR Location
                jobs = jobs.Where(s => s.Title.Contains(searchString)
                                       || s.Description.Contains(searchString)
                                       || s.Location.Contains(searchString));
            }

            return View(await jobs.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var jobOffer = await _context.JobOffers.FirstOrDefaultAsync(m => m.Id == id);
            if (jobOffer == null) return NotFound();

            return View(jobOffer);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Location,JobType,CreatedDate,Deadline,IsActive")] JobOffer jobOffer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobOffer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobOffer);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var jobOffer = await _context.JobOffers.FindAsync(id);
            if (jobOffer == null) return NotFound();
            return View(jobOffer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Location,JobType,CreatedDate,Deadline,IsActive")] JobOffer jobOffer)
        {
            if (id != jobOffer.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobOffer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobOfferExists(jobOffer.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(jobOffer);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var jobOffer = await _context.JobOffers.FirstOrDefaultAsync(m => m.Id == id);
            if (jobOffer == null) return NotFound();
            return View(jobOffer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobOffer = await _context.JobOffers.FindAsync(id);
            if (jobOffer != null) _context.JobOffers.Remove(jobOffer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobOfferExists(int id)
        {
            return _context.JobOffers.Any(e => e.Id == id);
        }
    }
}