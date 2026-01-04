using HRRecruitmentSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HRRecruitmentSystem.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // 1. Create Roles
            string[] roles = { "Admin", "HR", "Candidate" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // 2. Create Admin
            var adminEmail = "admin@hr.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var newAdmin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                await userManager.CreateAsync(newAdmin, "Admin@123");
                await userManager.AddToRoleAsync(newAdmin, "Admin");
                await userManager.AddToRoleAsync(newAdmin, "HR");
            }

            
            if (!context.JobOffers.Any(j => j.Title == "Senior .NET Developer"))
            {
                var jobs = new JobOffer[]
                {
                    new JobOffer { Title = "Senior .NET Developer", Description = "Lead backend team. C# & SQL.", Location = "Casablanca", JobType = JobType.FullTime, Deadline = DateTime.Now.AddDays(20), IsActive = true },
                    new JobOffer { Title = "Frontend React Dev", Description = "Build UI with React & Tailwind.", Location = "Rabat", JobType = JobType.FullTime, Deadline = DateTime.Now.AddDays(15), IsActive = true },
                    new JobOffer { Title = "HR Manager", Description = "Manage recruitment & relations.", Location = "Remote", JobType = JobType.Remote, Deadline = DateTime.Now.AddDays(30), IsActive = true },
                    new JobOffer { Title = "Data Intern", Description = "Learn PowerBI & SQL.", Location = "Tangier", JobType = JobType.Contract, Deadline = DateTime.Now.AddDays(10), IsActive = true },
                    new JobOffer { Title = "DevOps Engineer", Description = "Azure Pipelines & Docker.", Location = "Casablanca", JobType = JobType.FullTime, Deadline = DateTime.Now.AddDays(-1), IsActive = false } // Expired
                };
                context.JobOffers.AddRange(jobs);
                await context.SaveChangesAsync();
            }

            // 4. Seed Candidate & Applications (For Charts)
            if (!context.Candidates.Any(c => c.Email == "candidate@test.com"))
            {
                var userEmail = "candidate@test.com";
                var user = await userManager.FindByEmailAsync(userEmail);
                if (user == null)
                {
                    user = new IdentityUser { UserName = userEmail, Email = userEmail, EmailConfirmed = true };
                    await userManager.CreateAsync(user, "User@123");
                    await userManager.AddToRoleAsync(user, "Candidate");
                }

                var candidate = new Candidate
                {
                    UserId = user.Id,
                    FirstName = "Amine",
                    LastName = "Benali",
                    Email = userEmail,
                    PhoneNumber = "0600000000",
                    ResumePath = null
                };
                context.Candidates.Add(candidate);
                await context.SaveChangesAsync();

                // Add Fake Applications
                var netJob = context.JobOffers.FirstOrDefault(j => j.Title == "Senior .NET Developer");
                var reactJob = context.JobOffers.FirstOrDefault(j => j.Title == "Frontend React Dev");

                if (netJob != null && reactJob != null)
                {
                    context.Applications.Add(new Application { CandidateId = candidate.Id, JobOfferId = netJob.Id, Status = ApplicationStatus.Pending, AppliedAt = DateTime.Now.AddDays(-1) });
                    context.Applications.Add(new Application { CandidateId = candidate.Id, JobOfferId = reactJob.Id, Status = ApplicationStatus.Accepted, AppliedAt = DateTime.Now.AddDays(-5) });
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}