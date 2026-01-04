using HRRecruitmentSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HRRecruitmentSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // These lines tell Entity Framework to create these tables
        public DbSet<JobOffer> JobOffers { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Business Rule: A Candidate cannot apply to the same Job twice
            builder.Entity<Application>()
                .HasIndex(a => new { a.CandidateId, a.JobOfferId })
                .IsUnique();
        }
    }
}