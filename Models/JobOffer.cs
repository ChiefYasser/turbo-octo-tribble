using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace HRRecruitmentSystem.Models
{
    public class JobOffer
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Job Title")]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [Display(Name = "Job Type")]
        public JobType JobType { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.Date)]
        public DateTime Deadline { get; set; }

        public bool IsActive { get; set; } = true;

        // Relationship: One Job has many Applications
        public virtual ICollection<Application>? Applications { get; set; }
    }
}