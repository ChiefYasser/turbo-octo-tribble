using System.ComponentModel.DataAnnotations;

namespace HRRecruitmentSystem.Models
{
    public class Application
    {
        public int Id { get; set; }

        public int JobOfferId { get; set; }
        public virtual JobOffer? JobOffer { get; set; }

        public int CandidateId { get; set; }
        public virtual Candidate? Candidate { get; set; }

        public DateTime AppliedAt { get; set; } = DateTime.Now;

        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
    }
}